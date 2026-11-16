using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using NUnit.Framework;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    // Conditional from absorbed liquid puddles
    [SerializeField] private string _cond = "angry";
    private bool _isBindable = false;

    public string CurrentCondition => _cond;
    public string currentEffectActionBubble = "";
    public Color currentPheromoneColor;

    [SerializeField] GameObject bloodPreFab;

    public event Action<string> OnConditionChanged;

    void Awake()
    {
        // Singleton setup
        if (Instance)
            DestroyImmediate(this);
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Human"))
        {
            if (_isBindable)
            {
                // Then we are dropping a pheromone on them
                ActionItemBindingManager.Instance.SetBindingHuman(collision.gameObject.GetComponent<HumanManager>(), _cond, currentEffectActionBubble);
            }
            else
            {
                Debug.Log("Hit human with " + _cond + " effect");
                // Then its just an effect
                collision.gameObject.GetComponent<BehaviorManager>().SetState(_cond);
                if (bloodPreFab != null)
                {
                    Instantiate(bloodPreFab, GetComponent<Rigidbody>().transform);
                }
                return;
            }
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
            SetCondition(puddle.GetCondition());

            _isBindable = puddle.isPheromone;

            currentEffectActionBubble = puddle.currentEffectActionBubbleText;
            currentPheromoneColor = puddle.pheromoneColor;
        }
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
            return;

        _cond = newCondition;

        Debug.Log("Mosquito condition: " + _cond);

        OnConditionChanged?.Invoke(_cond);
    }
}
