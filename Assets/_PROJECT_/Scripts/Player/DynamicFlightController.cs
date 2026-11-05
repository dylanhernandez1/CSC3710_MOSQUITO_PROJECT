using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlightController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float pitchLimit = 80f;
    [SerializeField] float mouseSensitivity = 0.1f;

    [SerializeField] float maxFloatHeight = 10f;
    [SerializeField] float minFloatHeight = 0f;

    private Rigidbody rigidbody;
    InputAction look;
    float yaw, pitch;

    private Vector2 _movementVector;

    public Camera _freeLookCamera;


    Vector3 veloctiy;

    private InputAction moveForward;

    private float xRotation;

    //private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveForward = InputSystem.actions.FindAction("move");
        look = InputSystem.actions.FindAction("Look");
        yaw = transform.eulerAngles.y;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RotateWithMouse();
        rigidbody.linearVelocity = Vector3.zero;


        //Vector3 camForward = _freeLookCamera.transform.forward;


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.linearVelocity = Vector3.zero;
        Vector2 moveInput = moveForward.ReadValue<Vector2>();
        _movementVector.x = moveForward.ReadValue<Vector2>().x;
        _movementVector.y = moveForward.ReadValue<Vector2>().y;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            MoveCharacter(moveInput);
        }

    }

    private void MoveCharacter(Vector2 moveInput)
    {

        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x;
        dir = Vector3.ClampMagnitude(dir, 1f);
        Vector3 pos = transform.position + dir * moveSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minFloatHeight, maxFloatHeight);
        transform.position = pos;

    }

    private void DisableMovement()
    {
        //anim.SetBool("isFlying", false);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    private void RotateWithMouse()
    {
        Vector2 delta = look.ReadValue<Vector2>();
        yaw += delta.x * mouseSensitivity;
        pitch -= delta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
