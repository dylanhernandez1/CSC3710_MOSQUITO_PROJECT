
using UnityEngine;
using UnityEngine.Events;

public class SmackButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UnityEvent onButtonClicked;
    private bool pressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (pressed) return;

        if (other.CompareTag("HumanHand"))
        {
            pressed = true;
            onButtonClicked.Invoke();
        }
    }
}
