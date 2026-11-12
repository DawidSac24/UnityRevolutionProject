using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RevolutionController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [Header("Rewind Settings")]
    [SerializeField] private float rewindDurationInSeconds = 3f;
    [SerializeField] private int rewindSpeedMultiplier = 2;

    [Header("Ghost Settings")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private int ghostSpawnRate = 5;
    [SerializeField] private int ghostSpawnCount = 5;

    private const string ACTION_MAP = "Player";
    private const string REWIND_ACTION = "Rewind";
    private InputAction _rewindAction;

    private Rigidbody _rb;

    private readonly List<RecordedPoint> recordedPoints = new();
    private bool _isRewinding = false;

    private List<GameObject> _ghosts;
    private int _lastCloneCount;

    private void Awake()
    {
        _rewindAction = inputActions.FindActionMap(ACTION_MAP).FindAction(REWIND_ACTION);
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() => _rewindAction?.Enable();
    private void OnDisable() => _rewindAction?.Disable();

    private void Start()
    {
        _lastCloneCount = ghostSpawnRate;
        _ghosts = new List<GameObject>(ghostSpawnCount);
    }

    private void Update()
    {
        if (_rewindAction.triggered)
        {
            StartRewind();
        }
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
        if (recordedPoints.Count < rewindSpeedMultiplier)
        {
            StopRewind();
            return;
        }

        int lastIndex = recordedPoints.Count - rewindSpeedMultiplier;
        RecordedPoint lastRecord = recordedPoints[lastIndex];

        transform.position = lastRecord.position;
        transform.rotation = lastRecord.rotation;

        HandleGhostSpawn();

        for (int i = lastIndex; i < recordedPoints.Count; i++)
            recordedPoints.RemoveAt(i);
    }

    private void HandleGhostSpawn()
    {
        if (_ghosts.Count >= ghostSpawnCount)
        {
            GameObject ghost = _ghosts[0];
            Destroy(ghost);
            _ghosts.RemoveAt(0);
        }

        if (_lastCloneCount == 5)
        {
            Vector3 spawnPosition = new Vector3(_rb.position.x, 1, _rb.position.z);
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

        // Erase ghost momentum so the momentum from before the rewind isn't applied
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        DestroyAllGhosts();
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