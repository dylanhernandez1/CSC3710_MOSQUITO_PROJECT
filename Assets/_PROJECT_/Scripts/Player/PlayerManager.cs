using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    // Fields

    [Header("References")]
    [SerializeField] private GameObject _bloodPreFab;
    [SerializeField] private GameObject _absorbPrompt;


    // Conditional from absorbed liquid puddles
    [SerializeField] private string _cond = "angry";
    private bool _isBindable = false;

    private InputAction _interact;
    private Puddle _nearbyPuddle;


    public string CurrentCondition => _cond;
    public string currentEffectActionBubble = "";
    public Color currentPheromoneColor;

    // Events / Delegates

    public event Action<string> OnConditionChanged;

    public void Awake()
    {
        // Singleton setup
        if (Instance)
            DestroyImmediate(this);
        else
        {
            Instance = this;
        }

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
            Debug.Log(
                "Hit Human. Condition: " + _cond +
                " | Is Bindable: " + _isBindable
            );

            if (_isBindable)
            {
                ActionItemBindingManager.Instance.SetBindingHuman(
                    collision.gameObject.GetComponent<HumanManager>(),
                    _cond,
                    currentEffectActionBubble
                );
            }
            else
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
            }

            return;
        }
        // New action item
        else if (collision.gameObject.CompareTag("ActionItem"))
        {
            Debug.Log("Hir action item");
            if (!_isBindable)
            {
                Debug.Log("Not carrying a bindable pheromone condition, cannot infuse item");
                return;
            }

            ActionItem actionItem = collision.gameObject.GetComponent<ActionItem>();
            actionItem.bindingColor = currentPheromoneColor;

            //SetCondition(actionItem.currentEffect);
            ActionItemBindingManager.Instance.SetBindingActionItem(actionItem, _cond);
        }
        // Old puddle logic
        else if (collision.gameObject.TryGetComponent<Puddle>(out var puddle))
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
            return;

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

        _isBindable = _nearbyPuddle.isPheromone;

        currentEffectActionBubble =
            _nearbyPuddle.currentEffectActionBubbleText;

        currentPheromoneColor =
            _nearbyPuddle.pheromoneColor;

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