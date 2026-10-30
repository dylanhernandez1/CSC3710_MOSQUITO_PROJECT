using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StandbyBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform station;
    [SerializeField] private Animator _animator;
    private NavMeshAgent agent;

    [Header("Settings")]
    [SerializeField] private float standingDistance = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void StandBy()
    {
        float distance = Vector3.Distance(transform.position, station.position);
        if (distance > standingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(station.position);

            // Try to set the property for the blend tree (works safely with old human)
            try
            {
                _animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
            }
            catch (Exception e)
            {
                Debug.Log("Param Speed doesn't exist in animator: ");
                Debug.Log(e);
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
