using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System;

public class FollowBehavior : MonoBehaviour
{
    [Header("References")]
    private NavMeshAgent agent;
    private BehaviorManager _manager;
    private Animator _animator;
    [SerializeField] private Transform player;

    [Header("Settings")]
    public bool disableMovement = false;

    [SerializeField] private float followDistance = 2f;
    [SerializeField] private float maxFollowableHeight = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField, Range(0f, 180f)] private float fieldOfView = 90f;
    [SerializeField] private UnityEvent _attack;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _manager = GetComponent<BehaviorManager>();
        _animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }
    }

      
    public void Follow()
    {
        if (disableMovement || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        float heightDifference = Mathf.Abs(transform.position.y - player.position.y);

        if (distance < followDistance && heightDifference < maxFollowableHeight)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            _manager.SetState("default");
        }

        // Try to set the property for the blend tree
        try
        {
            _animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
        }
        catch (Exception e)
        {
            Debug.Log("Param Speed doesn't exist in animator");
            Debug.Log(e);
        }

        if (distance > attackRange || _attack == null)
            return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float angle = Vector3.Angle(transform.forward, toPlayer);

        if (angle <= fieldOfView * 0.5f)
        {
            _attack?.Invoke();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
