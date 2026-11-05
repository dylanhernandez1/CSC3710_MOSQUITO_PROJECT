using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class TogglePlayerManager : MonoBehaviour
{
    [SerializeField] CinemachineBrain brain;          // on Main Camera
    [SerializeField] MonoBehaviour noclipScript;      //  noclip script, on Main Camera
    [SerializeField] FlightController flight;         // on the bug

    public static event Action OnPlayerBite;

    // Conditional from absorbed liquid puddles
    private string _cond = "angry";
    public string CurrentCondition => _cond;

    bool noclip;

    [SerializeField] GameObject bloodPreFab;

    public event Action<string> OnConditionChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNoclip(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
            SetNoclip(!noclip);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Human"))
        {
            collision.gameObject.GetComponent<BehaviorManager>().SetState(_cond);
            if (bloodPreFab != null)
            {
                Instantiate(bloodPreFab, collision.contacts[0].point, Quaternion.identity);
            }
            OnPlayerBite?.Invoke();
            return;
        }

        Puddle puddle = collision.gameObject.GetComponent<Puddle>();
        if (puddle != null)
        {
            SetCondition(puddle.GetCondition());
        }
        Debug.Log("Hit " + collision.gameObject.name);
    }

    void SetNoclip(bool on)
    {
        noclip = on;
        brain.enabled = !on;        // Cinemachine lets go of the camera
        noclipScript.enabled = on;  // noclip takes over
        flight.enabled = !on;       // bug stops reacting to mouse/WASD

        // Reset orientation, kill leftover spin
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f); // upright, same facing

        // Flight's CharacterController shouldn't fight the Rigidbody in noclip mode
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = !on;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EndLevelTrigger>(out var trigger))
        {
            Debug.Log("Hit End level Trigger: " + other.gameObject.name);

            GameManager.Instance.SwitchScenes(trigger.GetNextSceneName());
        }
    }

    public void SetCondition(string newCondition)
    {
        if (_cond == newCondition)
        {
            return;
        }

        _cond = newCondition;

        Debug.Log("Mosquito condition: " + _cond);

        OnConditionChanged?.Invoke(_cond);
    }
}
