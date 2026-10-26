using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Fields

    [Header("References")]
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _verticalSpeed = 8f;

    [Header("Look")]
    [SerializeField] private float _mouseLookSensitivity = 0.12f;
    [SerializeField] private float _controllerLookSensitivity = 150f;
    [SerializeField] private float _minimumPitch = -80f;
    [SerializeField] private float _maximumPitch = 80f;

    private InputAction _move;
    private InputAction _look;
    private InputAction _flyUp;
    private InputAction _flyDown;

    private Vector2 _moveInput;
    private float _verticalInput;
    private float _yaw;
    private float _pitch;

    // Properties

    // Events / Delegates

    // Monobehaviour Methods (Awake, Start, Update, ...)

    private void Awake()
    {
        _rigidbody.useGravity = false;

        _move = InputSystem.actions.FindAction("Move");
        _look = InputSystem.actions.FindAction("Look");
        _flyUp = InputSystem.actions.FindAction("FlyUp");
        _flyDown = InputSystem.actions.FindAction("FlyDown");

        _yaw = transform.eulerAngles.y;

        LockCursor();
    }

    private void Update()
    {
        HandleInput();
        HandleLook();
        HandleCursor();
    }

    private void FixedUpdate()
    {
        Move();
        RotatePlayer();
    }

    // Public Methods

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Private Methods

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            _look.Reset();
        }
    }

    private void HandleInput()
    {
        _moveInput = _move.ReadValue<Vector2>();

        _verticalInput =
            (_flyUp.IsPressed() ? 1f : 0f) -
            (_flyDown.IsPressed() ? 1f : 0f);
    }

    private void HandleLook()
    {
        if (!Application.isFocused ||
            Cursor.lockState != CursorLockMode.Locked)
        {
            _look.Reset();
            return;
        }

        Vector2 lookInput = _look.ReadValue<Vector2>();

        bool usingController =
            _look.activeControl != null &&
            _look.activeControl.device is Gamepad;

        float sensitivity = usingController
            ? _controllerLookSensitivity * Time.deltaTime
            : _mouseLookSensitivity;

        _yaw += lookInput.x * sensitivity;

        _pitch = Mathf.Clamp(
            _pitch - lookInput.y * sensitivity,
            _minimumPitch,
            _maximumPitch
        );

        if (_cameraPivot != null)
        {
            _cameraPivot.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
    }

    private void HandleCursor()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            Cursor.lockState != CursorLockMode.Locked)
        {
            LockCursor();
        }
    }

    private void Move()
    {
        Vector3 cameraForward = _cameraPivot.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = _cameraPivot.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 horizontalMovement =
            cameraForward * _moveInput.y +
            cameraRight * _moveInput.x;

        if (horizontalMovement.sqrMagnitude > 1f)
        {
            horizontalMovement.Normalize();
        }

        _rigidbody.linearVelocity =
            horizontalMovement * _moveSpeed +
            Vector3.up * _verticalInput * _verticalSpeed;
    }

    private void RotatePlayer()
    {
        Vector3 movementDirection = new Vector3(
            _rigidbody.linearVelocity.x,
            0f,
            _rigidbody.linearVelocity.z
        );

        if (movementDirection.sqrMagnitude < 0.01f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(movementDirection, Vector3.up);

        _rigidbody.MoveRotation(targetRotation);
    }
}