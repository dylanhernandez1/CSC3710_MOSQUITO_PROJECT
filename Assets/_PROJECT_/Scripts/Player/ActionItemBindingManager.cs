using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class ActionItemBindingManager : MonoBehaviour
{
    public static ActionItemBindingManager Instance;

    // Pheromone -> human - item binding
    private Dictionary<string, (ActionItem item, HumanManager human)> actionItemBindings = new();

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
        // Remove this item from any old effect binding.
        foreach (string key in new List<string>(actionItemBindings.Keys))
        {
            if (key == effect)
                continue;

            var binding = actionItemBindings[key];

            if (binding.item == item)
            {
                binding.item = null;
                actionItemBindings[key] = binding;
            }
        }

        // Get or create this effect's binding.
        if (!actionItemBindings.TryGetValue(effect, out var currentBinding))
            currentBinding = (null, null);

        // Clear the old item's record if it is being replaced.
        if (currentBinding.item != null && currentBinding.item != item)
            currentBinding.item.currentEffect = null;

        currentBinding.item = item;
        actionItemBindings[effect] = currentBinding;

        item.currentEffect = effect;

        // TODO: If currentBinding.human exists, tell it to pathfind to item here.
        DebugAll();
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
        DebugAll();
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

    public void DebugAll()
    {
        foreach(var item in actionItemBindings)
        {
            Debug.Log(item);
        }
    }
}