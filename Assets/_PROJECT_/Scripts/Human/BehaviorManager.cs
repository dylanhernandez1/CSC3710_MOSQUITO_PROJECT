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
    public string stateName;
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

    public HumanManager humanManager;

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
        }
        else
        {
            _events[_state].behavior?.Invoke();
        }
    }

    public void SetState(string stateName)
    {
        string requestedState = stateName.Trim().ToLowerInvariant();

        if ((requestedState == "default" || requestedState == "") &&
            _events.Count != 0)
        {
            _state = 0;

            Debug.Log(
                gameObject.name +
                " changed to default behavior."
            );

            return;
        }

        for (int i = 0; i < _events.Count; i++)
        {
            string configuredState =
                _events[i].stateName.Trim().ToLowerInvariant();

            if (configuredState == requestedState)
            {
                _state = i;

                Debug.Log(
                    gameObject.name +
                    " changed behavior to: " +
                    configuredState
                );

                if (_animator != null)
                {
                    _animator.SetTrigger("Walk");
                }

                if (_agent != null)
                {
                    _agent.isStopped = false;
                    _agent.updateRotation = true;
                }

                return;
            }
        }

        Debug.LogWarning(
            gameObject.name +
            " has no configured behavior for condition: " +
            requestedState
        );
    }

    public int GetState()
    {
        return _state;
    }
}
