using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEditor.PackageManager;

[Serializable]
public struct condEvent
{
    public string condition;
    public UnityEvent behavior;
}

public class BehaviorManager : MonoBehaviour
{
    /* 
     * Replaced default/secondary behaviors with a fully customizable list of behaviors and associated conditions
     * Instead of incrementing the state machine, you should use conditionals to switch between states
     */

    [SerializeField] private int _state = 0;
    [SerializeField] private List<condEvent> _events = new();
    

    private NavMeshAgent _agent;
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        if (_events.Count == 0)
        {
            Debug.LogError("BehaviorManager has zero behaviors");
        }
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

        if (_events.Count <= _state)
        {
            Debug.LogError("BehaviorManager tried to play a state that doesn't exist");
        } else
        {
            _events[_state].behavior?.Invoke();
        }
    }

    public void SetState(string cond)
    {
        if ((cond == "default" || cond == "") && _events.Count != 0)
        {
            _state = 0;
            return;
        }

        for (int i = 0; i < _events.Count; i++)
        {
            condEvent _event = _events[i];

            if (_event.condition == cond)
            {
                _state = i;
                _animator.SetTrigger("Walk");
                _agent.isStopped = false;
                _agent.updateRotation = true;
                return;
            }
        }
    }

    public int GetState()
    {
        return _state;
    }
}
