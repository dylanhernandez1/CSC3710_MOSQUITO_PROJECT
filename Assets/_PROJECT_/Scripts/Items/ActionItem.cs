using UnityEngine;

public class ActionItem : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Stations tags as this are where the human can use this item")]
    public string usableStationsTag = "";
    
    [Header("State")]
    public string currentEffect;

    public bool isBindable = false;
    public bool isBeingHeld = false;

    [Header("Refs")]
    public LineRenderer bindingLine;
    public Color bindingColor;

    void Update()
    {
        if (ActionItemBindingManager.Instance.TryGetHuman(this, out HumanManager man))
        {
            bindingLine.enabled = true;

            bindingLine.SetPosition(0, transform.position);

            if (isBeingHeld)
                bindingLine.SetPosition(1, man.actionItemBehavior.station.transform.position); // Station
            else
                bindingLine.SetPosition(1, man.transform.position); // Human

            // Set the colors of the line
            bindingLine.colorGradient = new Gradient
            {
                colorKeys = new[]
                {
                    new GradientColorKey(bindingColor, 0f),
                    new GradientColorKey(bindingColor, 1f)
                }
            };
        }
        else
        {
            bindingLine.enabled = false;
        }
    }
}
