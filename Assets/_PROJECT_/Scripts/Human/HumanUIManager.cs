using UnityEngine;
using TMPro;
using System.Drawing;

public class HumanUIManager : MonoBehaviour
{
    [Header("Action Bubble")]
    [Header("Refs")]
    private HumanManager _manager;
    private Transform _playerTransform;

    public Transform actionBubbleCanvas;
    public TMP_Text actionBubbleText;

    [Header("State")]
    public bool isActionBubbleShown = true;
    public string actionBubbleContent = "...";

    void Awake()
    {
        _manager = GetComponent<HumanManager>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isActionBubbleShown)
            UpdateActionBubbleRotation();
    }

    void UpdateActionBubbleRotation()
    {
        actionBubbleCanvas.LookAt(_playerTransform);
    }
    
    /// <summary>
    /// Sets the action buttons button gameobject to in/active
    /// </summary>
    public void SetActionBubbleShown(bool isShown)
    {
        isActionBubbleShown = isShown;
        actionBubbleCanvas.gameObject.SetActive(isShown);
    }

    // Util funcs
    public void ShowActionBubble() => SetActionBubbleShown(true);
    public void HideActionBubble() => SetActionBubbleShown(false);


    public void SetActionBubbleContent(string content)
    {
        actionBubbleContent = content;
        actionBubbleText.text = actionBubbleContent;
    } 
}
