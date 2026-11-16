using UnityEngine;

public class ScrewDriverVentStation : ActionItemStation
{
    // Implement the interact feature
    public override void Interact()
    {
        Debug.Log("Station was interacted with");
    }
}