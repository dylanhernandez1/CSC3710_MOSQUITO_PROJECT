using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // Fields

    [Header("References")]
    [SerializeField] private GameObject _bloodPreFab;
    [SerializeField] private GameObject _absorbPrompt;

    // Conditional from absorbed liquid puddles
    private string _cond = "angry";

    private InputAction _interact;
    private Puddle _nearbyPuddle;

    // Properties

    public string CurrentCondition => _cond;

    // Events / Delegates

    public event Action<string> OnConditionChanged;

    // Monobehaviour Methods (Awake, Start, Update, ...)

    private void Awake()
    {
        _interact = InputSystem.actions.FindAction("Interact");

        SetAbsorbPromptVisible(false);
    }

    private void Update()
    {
        TryAbsorbPuddle();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Human"))
        {
            collision.gameObject
                .GetComponent<BehaviorManager>()
                .SetState(_cond);

            if (_bloodPreFab != null)
            {
                Instantiate(
                    _bloodPreFab,
                    GetComponent<Rigidbody>().transform
                );
            }

            return;
        }

        Puddle puddle =
            collision.gameObject.GetComponentInParent<Puddle>();

        if (puddle != null)
        {
            _nearbyPuddle = puddle;

            SetAbsorbPromptVisible(true);

            Debug.Log(
                "Near puddle. Press Interact to absorb: " +
                puddle.GetCondition()
            );
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Puddle puddle =
            collision.gameObject.GetComponentInParent<Puddle>();

        if (puddle == _nearbyPuddle)
        {
            _nearbyPuddle = null;

            SetAbsorbPromptVisible(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EndLevelTrigger>(out var trigger))
        {
            Debug.Log(
                "Hit End level Trigger: " +
                other.gameObject.name
            );

            GameManager.Instance.SwitchScenes(
                trigger.GetNextSceneName()
            );
        }
    }

    // Public Methods

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

    // Private Methods

    private void TryAbsorbPuddle()
    {
        if (_nearbyPuddle == null ||
            !_interact.WasPressedThisFrame())
        {
            return;
        }

        SetCondition(_nearbyPuddle.GetCondition());

        Debug.Log(
            "Absorbed puddle: " +
            _nearbyPuddle.GetCondition()
        );

        // Uncomment this if each puddle should disappear after use.
        // Destroy(_nearbyPuddle.gameObject);

        _nearbyPuddle = null;

        SetAbsorbPromptVisible(false);
    }

    private void SetAbsorbPromptVisible(bool isVisible)
    {
        if (_absorbPrompt != null)
        {
            _absorbPrompt.SetActive(isVisible);
        }
    }
}