using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    // Conditional from absorbed liquid puddles
    private string _cond = "angry";
    private bool _isBindable = false;

    public string CurrentCondition => _cond;

    [SerializeField] GameObject bloodPreFab;

    public event Action<string> OnConditionChanged;

    // FOR BINDING ITEMS TO HUMANS
    Dictionary<string, (ActionItem item, HumanManager human)> actionItemBindings= new();

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
            collision.gameObject.GetComponent<BehaviorManager>().SetState(_cond);
            if (bloodPreFab != null)
            {
                Instantiate(bloodPreFab, GetComponent<Rigidbody>().transform);
            }
            return;
        }
        // New action item
        else if (collision.gameObject.CompareTag("ActionItem"))
        {
            ActionItem actionItem = collision.gameObject.GetComponent<ActionItem>();

            //SetCondition(actionItem.currentEffect);
            _isBindable = actionItem.isBindable;

            if (actionItem.isBindable)
                SetBindingActionItem(actionItem);
        }
        // Old puddle logic
        else if (collision.gameObject.TryGetComponent<Puddle>(out var puddle))
        {
            SetCondition(puddle.GetCondition());
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

    public void SetBindingActionItem(ActionItem item)
    {
        string effect = item.currentEffect;

        if (actionItemBindings.ContainsKey(effect))
        {
            if (actionItemBindings[effect].item)
            {
                /// If theres already an item
                
                /// Swap items in the binding
                // Remove the binding from the old item
                actionItemBindings[effect].item.currentEffect = null;

                // Change binding to new item
                actionItemBindings[effect] = (item, actionItemBindings[effect].human);

                /// Swap out the item that the human is looking for
            }
            

            actionItemBindings[effect].human?.behaviorManager.SetState("default");

            // Remove binding from the list
            actionItemBindings.Remove(effect);
        }

        // Add the binding
        actionItemBindings.Add(effect, (item, null));
    }

    public void SetBindingHuman(HumanManager human, string effect)
    {
        if (actionItemBindings.ContainsKey(effect))
        {
            // Remove the binding from the human
            if (actionItemBindings[effect].human)
                actionItemBindings[effect].human.behaviorManager.SetState("default");

            // Swap out the human for this human
            actionItemBindings[effect] = (actionItemBindings[effect].item, human);
        }
        else
        {
            // Add the new binding for this human
            actionItemBindings.Add(effect, (null, human));
        }
    }
}
