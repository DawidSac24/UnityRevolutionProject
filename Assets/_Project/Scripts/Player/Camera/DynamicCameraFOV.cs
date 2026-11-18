using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCameraFOV : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody playerRb;

    [Header("FOV Settings")]
    [SerializeField] private float baseFov = 90f;
    [SerializeField] private float maxFov = 110f;
    [SerializeField] private float smoothSpeed = 8f;

    [Header("Speed Thresholds")]
    [Tooltip("Speed at which FOV starts increasing (e.g., your walk speed)")]
    [SerializeField] private float minSpeed = 10f;
    [Tooltip("Speed at which FOV hits maxFov (e.g., dash or fast slide speed)")]
    [SerializeField] private float maxSpeed = 30f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (playerRb == null) return;

        // 1. Measure horizontal speed (ignores falling so jumping doesn't warp FOV)
        Vector3 horizontalVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        // 2. Convert speed into a 0-to-1 percentage, then map it to your FOV range
        float speedPercent = Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed);
        float targetFov = Mathf.Lerp(baseFov, maxFov, speedPercent);

        // 3. Smoothly transition the camera's FOV
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFov, smoothSpeed * Time.deltaTime);
    }
}