using System.Collections.Generic;
using UnityEngine;

public readonly struct RecordedPoint
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }

    public RecordedPoint(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}

public class RevolutionController : MonoBehaviour
{
    [Header("Rewind Settings")]
    [SerializeField] private float rewindCooldown = 3f;
    [SerializeField] private float rewindDurationInSeconds = 3f;
    [SerializeField] private int rewindSpeedMultiplier = 2;
    [SerializeField] private bool enableRewindCameraOverwrite = true;

    [Header("Ghost Effect")]
    [SerializeField] private bool enableGhostEffect = true;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private int ghostSpawnRate = 5;
    [SerializeField] private int ghostSpawnCount = 5;
    [SerializeField] private float ghostSpawnYOffset = 1f;

    public float RewindCooldown => rewindCooldown;
    public int RewindSpeedMultiplier => rewindSpeedMultiplier;
    public bool EnableRewindCameraOverwrite => enableRewindCameraOverwrite;
    public bool EnableGhostEffect => enableGhostEffect;
    public GameObject GhostPrefab => ghostPrefab;
    public int GhostSpawnRate => ghostSpawnRate;
    public int GhostSpawnCount => ghostSpawnCount;
    public float GhostSpawnYOffset => ghostSpawnYOffset;

    public bool HasRecordedPoints => _recordedPoints.Count > rewindSpeedMultiplier;
    public bool IsRecording { get; set; } = true;

    private readonly List<RecordedPoint> _recordedPoints = new();

    private void FixedUpdate()
    {
        if (!IsRecording) return;

        int maxFrames = Mathf.RoundToInt(rewindDurationInSeconds / Time.fixedDeltaTime);
        if (_recordedPoints.Count >= maxFrames)
        {
            _recordedPoints.RemoveAt(0);
        }

        _recordedPoints.Add(new RecordedPoint(transform.position, transform.rotation));
    }

    public bool TryPopRewindPoint(out RecordedPoint point)
    {
        if (_recordedPoints.Count <= rewindSpeedMultiplier)
        {
            point = default;
            return false;
        }

        int targetIndex = _recordedPoints.Count - rewindSpeedMultiplier;
        point = _recordedPoints[targetIndex];
        _recordedPoints.RemoveRange(targetIndex, _recordedPoints.Count - targetIndex);
        return true;
    }

    public void Clear()
    {
        _recordedPoints.Clear();
    }
}