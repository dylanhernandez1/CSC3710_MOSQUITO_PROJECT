using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BehaviorManager : MonoBehaviour
{
    [SerializeField] private UnityEvent defaultBehavior;
    [SerializeField] private UnityEvent secondaryBehavior;
    [SerializeField] private UnityEvent tertiaryBehavior;

    [SerializeField] private string secondaryCond = "secondary";
    [SerializeField] private string tertiaryCond = "tertiary";

    [SerializeField] private int _state = 1;

    private NavMeshAgent _agent;
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Try to set the property for the blend tree
        try
        {
            _animator.SetFloat("Speed", _agent.velocity.magnitude / _agent.speed);
        }
        catch (Exception e)
        {
            Debug.Log("Param Speed doesn't exist in animator");
            Debug.Log(e);
        }
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
            return;
        }
        _animator.SetTrigger("Walk");
        _agent.isStopped = false;
        _agent.updateRotation = true;
    }

    public int GetState()
    {
        return _state;
    }
}
