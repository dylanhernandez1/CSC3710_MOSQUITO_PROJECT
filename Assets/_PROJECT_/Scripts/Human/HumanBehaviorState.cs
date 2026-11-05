using NUnit.Framework;
using UnityEngine;

[System.Serializable]
public abstract class HumanBehaviorState
{
    [Header("Refs")]
    public BehaviorManager behaviorManager;
    
    public string stateName;
    public string stateActionBubbleContent = "...";

    /// <summary>
    /// Runs once when the scene is loaded.
    /// Essentially an Awake() method for states.
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// Runs once per frame when state is active
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// Util method for subclasses to set the action bubble content
    /// </summary>
    public void SetActionBubbleContent(string content) => behaviorManager.humanManager.uIManager.SetActionBubbleContent(content);

    /// <summary>
    /// Util method for subclasses to set the action bubble status
    /// </summary>
    public void SetActionBubbleState(bool isShown) => behaviorManager.humanManager.uIManager.SetActionBubbleShown(isShown);
}