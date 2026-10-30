using UnityEngine;
using UnityEngine.Events;

public class BehaviorManager : MonoBehaviour
{
    [SerializeField] private UnityEvent defaultBehavior;
    [SerializeField] private UnityEvent secondaryBehavior;
    [SerializeField] private UnityEvent tertiaryBehavior;

    [SerializeField] private string secondaryCond = "secondary";
    [SerializeField] private string tertiaryCond = "tertiary";

    [SerializeField] private int _state = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == 1)
        {
            defaultBehavior?.Invoke();
        } else if (_state == 2)
        {
            secondaryBehavior?.Invoke();
        } else if (_state == 3)
        {
            tertiaryBehavior?.Invoke();
        }
    }

    public void SetState(string cond)
    {
        if (cond == "default" || cond == "")
        {
            _state = 1;
        } else if (cond == secondaryCond)
        {
            _state = 2;
        } else if (cond == tertiaryCond)
        {
            _state = 3;
        } else
        {
            Debug.Log("Unknown conditional for behavior manager");
        }
    }
}
