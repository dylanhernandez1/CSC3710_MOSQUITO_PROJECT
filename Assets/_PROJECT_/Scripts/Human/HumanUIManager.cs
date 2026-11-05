using UnityEngine;
using TMPro;
using System.Drawing;

public class HumanUIManager : MonoBehaviour
{
    [Header("Action Bubble")]
    [Header("Refs")]
    public Transform actionBubble;
    public TMP_Text actionBubbleText;

    [Header("State")]
    public bool isActionBubbleShown = true;
    public string actionBubbleContent = "...";


    /// <summary>
    /// Sets the action buttons button gameobject to in/active
    /// </summary>
    public void SetActionBubbleShown(bool isShown)
    {
        isActionBubbleShown = isShown;
        actionBubble.gameObject.SetActive(isShown);
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
