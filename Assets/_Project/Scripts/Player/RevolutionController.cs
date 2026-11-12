using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RevolutionController : MonoBehaviour
{
    [Header("Rewind Settings")]
    [SerializeField] private float rewindDurationInSeconds = 3f;
    [SerializeField] private int rewindSpeedMultiplier = 2;

    [Header("Ghost Effect")]
    [SerializeField] private bool enableGhostEffect = true;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private int ghostSpawnRate = 5;
    [SerializeField] private int ghostSpawnCount = 5;
    [SerializeField] private float ghostSpawnYOffset = 1f;

    private Rigidbody _rb;
    private PlayerInputHandler _inputHandler;

    private readonly List<RecordedPoint> recordedPoints = new();
    private bool _isRewinding = false;

    private List<GameObject> _ghosts;
    private int _lastCloneCount;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        
        _lastCloneCount = ghostSpawnRate;
        _ghosts = new List<GameObject>(ghostSpawnCount);
    }

    private void FixedUpdate()
    {
        if (_isRewinding)
        {
            HandleRevolution();
        }
        else
        {
            Record();
        }
    }

    private void Record()
    {
        // Calculate max frames based on Unity's physics rate
        // 3 seconds * 50 = 150 points.
        int maxFrames = Mathf.RoundToInt(rewindDurationInSeconds / Time.fixedDeltaTime);

        if (recordedPoints.Count >= maxFrames)
        {
            recordedPoints.RemoveAt(0);
        }

        recordedPoints.Add(new RecordedPoint(transform.position, transform.rotation));
    }

    private void HandleRevolution()
    {
        if (recordedPoints.Count <= rewindSpeedMultiplier)
        {
            StopRewind();
            return;
        }

        int lastIndex = recordedPoints.Count - rewindSpeedMultiplier;
        RecordedPoint lastRecord = recordedPoints[lastIndex];

        _rb.MovePosition(lastRecord.position);
        _rb.MoveRotation(lastRecord.rotation);

        HandleGhostSpawn();

        recordedPoints.RemoveRange(lastIndex, recordedPoints.Count - lastIndex);
    }

    private void HandleGhostSpawn()
    {
        if (!enableGhostEffect) return;
        if (recordedPoints.Count <= rewindSpeedMultiplier) { StopRewind(); return; }
        if (_ghosts.Count >= ghostSpawnCount)
        {
            GameObject ghost = _ghosts[0];
            Destroy(ghost);
            _ghosts.RemoveAt(0);
        }

        if (_lastCloneCount == 5)
        {
            Vector3 spawnPosition = new Vector3(_rb.position.x, _rb.position.y + ghostSpawnYOffset, _rb.position.z);
            _ghosts.Add(Instantiate(ghostPrefab, spawnPosition, transform.rotation));

            _lastCloneCount = 0;
        }

        _lastCloneCount++;
    }

    private void DestroyAllGhosts()
    {
        foreach (GameObject ghost in _ghosts)
        {
            Destroy(ghost);
        }

        _ghosts.Clear();
    }

    private void StartRewind()
    {
        if (recordedPoints.Count == 0) return;
        _isRewinding = true;

        _rb.isKinematic = true;
    }

    private void StopRewind()
    {
        _isRewinding = false;
        _rb.isKinematic = false;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        recordedPoints.Clear(); 

        DestroyAllGhosts();
    }
    
    private void OnEnable()
    {
        PlayerInputHandler.OnRewindPressed += StartRewind;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnRewindPressed -= StartRewind;
    }

    private class RecordedPoint
    {
        public Vector3 position { get; }
        public Quaternion rotation { get; }

        public RecordedPoint(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}