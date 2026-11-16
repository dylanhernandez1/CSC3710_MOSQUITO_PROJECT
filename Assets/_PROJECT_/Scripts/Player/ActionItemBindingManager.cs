using UnityEngine;
using System.Collections.Generic;

public class ActionItemBindingManager : MonoBehaviour
{
    public static ActionItemBindingManager Instance;

    // Pheromone -> human - item binding
    Dictionary<string, (ActionItem item, HumanManager human)> actionItemBindings= new();

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public void SetBindingActionItem(ActionItem item, string effect)
    {
        // Check for existing binding
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

                // Set new items effect
                item.currentEffect = effect;

                /// TODO Swap out the item that the human is looking for
            }
            else
                actionItemBindings[effect] = (item, actionItemBindings[effect].human);
        }
        else
        {
            // Add the binding
            actionItemBindings.Add(effect, (item, null));
        }
    }

    public void SetBindingHuman(HumanManager human, string effect)
    {
        // A human can only follow one pheromone effect at a time.
        foreach (string key in new List<string>(actionItemBindings.Keys))
        {
            if (key == effect)
                continue;

            var binding = actionItemBindings[key];

            if (binding.human == human)
            {
                binding.human = null;
                actionItemBindings[key] = binding;
            }
        }

        if (!actionItemBindings.TryGetValue(effect, out var currentBinding))
            currentBinding = (null, null);

        // Another human was following this effect; stop them.
        if (currentBinding.human != null && currentBinding.human != human)
            currentBinding.human.behaviorManager.SetState("default");

        currentBinding.human = human;
        actionItemBindings[effect] = currentBinding;

        // TODO: If currentBinding.item exists, tell human to pathfind to it here.
    }

    public (ActionItem, HumanManager) GetBinding(string pheromoneEffect)
    {
        if (actionItemBindings.TryGetValue(pheromoneEffect, out (ActionItem, HumanManager) val))
        {
            return val;
        }
        else
            return (null, null);
    }
}