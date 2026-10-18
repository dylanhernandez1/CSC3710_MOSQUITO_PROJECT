using UnityEngine;
using UnityEngine.InputSystem;

public class AlwaysNoclip : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 8f;

    [Header("Camera")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 2f, -5f);
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 70f;

    private float yaw;
    private float pitch;

    private void Awake()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;

        yaw = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        Vector3 move = Vector3.zero;

        if (keyboard.aKey.isPressed) move -= transform.right;
        if (keyboard.dKey.isPressed) move += transform.right;
        if (keyboard.wKey.isPressed) move += transform.forward;
        if (keyboard.sKey.isPressed) move -= transform.forward;
        if (keyboard.spaceKey.isPressed) move += Vector3.up;
        if (keyboard.leftShiftKey.isPressed) move += Vector3.down;

        if (move.sqrMagnitude > 0f)
            transform.position += move.normalized * speed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (playerCamera == null || Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Face the player in the direction the camera is orbiting.
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        playerCamera.position = transform.position + rotation * cameraOffset;
        playerCamera.rotation = rotation;
    }
}