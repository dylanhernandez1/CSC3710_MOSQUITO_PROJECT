using UnityEngine;

public class ActionItem : MonoBehaviour
{
    public string currentEffect;

    public bool isBindable = false;

    public LineRenderer bindingLine;

    void Update()
    {
        if (ActionItemBindingManager.Instance.TryGetHuman(this, out HumanManager man))
        {
            bindingLine.enabled = true;

            bindingLine.SetPosition(0, transform.position);
            bindingLine.SetPosition(1, man.transform.position);
        }
        else
        {
            bindingLine.enabled = false;
        }
    }
}
