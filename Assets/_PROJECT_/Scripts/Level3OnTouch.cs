using UnityEngine;
using UnityEngine.Events;

public class Level3OnTouch : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _event.Invoke();
        }
    }
}
