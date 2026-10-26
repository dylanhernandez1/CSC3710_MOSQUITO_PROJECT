
using UnityEngine;

public class SmackButton : MonoBehaviour
{
    [SerializeField] private SlidingDoor door;
    private bool pressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (pressed) return;

        if (other.CompareTag("HumanHand"))
        {
            pressed = true;
            door.Open();
        }
    }
}
