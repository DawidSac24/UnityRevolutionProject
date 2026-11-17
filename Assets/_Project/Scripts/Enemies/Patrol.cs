using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour
{
    private enum State
    {
        patrol,
        chase
    }

    private NavMeshAgent _agent;
    [SerializeField] private List<Transform> wayPoints;
    private int _index = 0;

    private const string TAG_PLAYER = "Player";
    private GameObject _player;
    private bool _playerFound = false;
    private Light _light;
    private State _currentState = State.patrol;


    private Vector3 GetNextDestination()
    {
        Transform checkpoint = wayPoints[_index];
        _index = (_index + 1) % wayPoints.Count;
        return checkpoint.position;
    }

    private void SearchPlayer()
    {
        if (_player == null) return;

        Vector3 origin = transform.position + 1.5f * Vector3.up;
        Vector3 destination = _player.transform.position + 1.5f * Vector3.up;

        Vector3 direction = (destination - origin).normalized;

        float angle = Vector3.Angle(transform.forward, direction);
        float distance = Vector3.Distance(origin, destination);

        // DEBUG — à supprimer une fois le problème résolu
        Debug.DrawRay(origin, direction * 10f, Color.yellow);
        Debug.Log($"Angle: {angle:F1}° | Distance: {distance:F1}m | Dans le cône: {angle <= 170f && distance < 10f}");

        _playerFound = false;

        if (angle <= 140f && distance < 10f)
        {
            Ray ray = new Ray(origin, direction);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Transform hitTransform = hit.collider.transform;
                bool tagFound = hitTransform.CompareTag(TAG_PLAYER) ||
                                (hitTransform.parent != null && hitTransform.parent.CompareTag(TAG_PLAYER));
                if (tagFound)
                {
                    _playerFound = true;
                }
            }
        }

        _currentState = _playerFound ? State.chase : State.patrol;
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(GetNextDestination());

        var players = GameObject.FindGameObjectsWithTag(TAG_PLAYER);
        if (players.Length == 0)
        {
            Debug.LogError("Patrol component need a separate GameObject tagged " + TAG_PLAYER + " in the scene.");
            return;
        }
        _player = players[0];

        _light = GetComponentInChildren<Light>();
    }

    void Update()
    {
        SearchPlayer();

        switch (_currentState)
        {
            case State.patrol:
                PatrolUpdate();
                break;
            case State.chase:
                ChaseUpdate();
                break;
            default: PatrolUpdate(); break;
        }
    }
    
    private void PatrolUpdate()
    {
        if (_light != null) _light.color = Color.green;
        if (_agent.pathPending) return;
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.destination = GetNextDestination();
        }
    }

    private void ChaseUpdate()
    {
        if (_light != null) _light.color = Color.red;
        _agent.SetDestination(_player.transform.position);
    }
}