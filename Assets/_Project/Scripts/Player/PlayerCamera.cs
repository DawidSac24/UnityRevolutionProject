using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Look Settings")]
    [SerializeField] private float lookSensitivity = 0.2f;
    [SerializeField] private float lookAngleLimit = 90f;

    [Header("Camera Settings")]
    [SerializeField] private float fieldOfView = 60.0f;
    private Camera _mainCamera;

    private PlayerInputHandler _inputHandler;
    private Rigidbody _rb;

    // private CharacterController cc;


    private float _lookAngle = 0.0f;

    void Start()
    {
        _mainCamera = GetComponentInChildren<Camera>();
        _inputHandler = GetComponentInParent<PlayerInputHandler>();
        _rb = GetComponentInParent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLooking(_inputHandler.LookInput);
        _mainCamera.fieldOfView = fieldOfView;
    }

    private void HandleLooking(Vector2 lookDelta)
    {
        _lookAngle += -lookDelta.y * lookSensitivity;
        _lookAngle = Mathf.Clamp(_lookAngle, -lookAngleLimit, lookAngleLimit);
        _mainCamera.transform.localRotation = Quaternion.Euler(_lookAngle, 0, 0);

        _rb.transform.Rotate(Vector3.up * (lookDelta.x * lookSensitivity));
    }
}