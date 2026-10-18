using UnityEngine;
using UnityEngine.Events;

public class BehaviorManager : MonoBehaviour
{
    [SerializeField] private UnityEvent defaultBehavior;
    [SerializeField] private UnityEvent secondaryBehavior;
    [SerializeField] private UnityEvent tertiaryBehavior;

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

    public void SetToDefault()
    {
        _state = 1;
    }

    public void SetToSecondary()
    {
        _state = 2;
    }

    public void SetToTertiary()
    {
        _state = 3;
    }

    public void SetToDisable()
    {
        _state = 0;
    }
}
