using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StandbyBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform station;
    private NavMeshAgent agent;

    [Header("Settings")]
    [SerializeField] private float standingDistance = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StandBy()
    {
        float distance = Vector3.Distance(transform.position, station.position);
        if (distance > standingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(station.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
