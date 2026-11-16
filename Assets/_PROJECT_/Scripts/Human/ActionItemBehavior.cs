using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;

public class ActionItemBehavior : MonoBehaviour
{
    [Header("References")]
    private NavMeshAgent agent;
    private BehaviorManager _manager;
    private Animator _animator;
    [SerializeField] private Transform player;

    private ActionItem targetItem;

    [Header("Settings")]
    public bool disableMovement = false;
    
    public float distanceToPickUpItem = 2.5f;
    public float distanceToInteractWithStation = 2.5f;

    [Header("State")]  
    public bool isHoldingItem = false;
    public string currentActionBubbleText = "?";

    public ActionItemStation station;


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

      
    public void UpdateState()
    {
        _manager.humanManager.uIManager.SetActionBubbleContent(currentActionBubbleText);

        // Try to get the newest target item
        if (!ActionItemBindingManager.Instance.TryGetItem(_manager.humanManager, out targetItem))
            return; // No item bound
        
        if (disableMovement || player == null)
            return;

        if (station != null)
        {
            if (Vector3.Distance(transform.position, station.transform.position) < distanceToInteractWithStation)
            {
                // Interact with station
                Debug.Log($"[{name}] Interacting with station [{station.name}]");
                station.Interact();

                // TODO: add some animations, dont just go back to standing there
                _manager.SetState("default");
            }
            else
            {
                // Move to station logic
                agent.isStopped = false;
                agent.SetDestination(station.transform.position);
            }
        }
        else
        {
            // Move to item logic
            if (Vector3.Distance(transform.position, targetItem.transform.position) < distanceToPickUpItem)
            {
                // Stop this human
                agent.isStopped = true;

                // Is at item, pick it up and use it
                Debug.Log($"[{name}] Picking up item [{targetItem.name}]");
                isHoldingItem = true;

                FindNearestUsableStation();

                if (station == null)
                {
                    // There are no usable stations nearby, abort
                    Debug.Log($"There are no usable stations nearby with the given tag: {targetItem.usableStationsTag}");

                    return;
                }
            }
            else
            {
                // Too far, move to item
                agent.isStopped = false;
                agent.SetDestination(targetItem.transform.position);
            }
        }
    }

    public void FindNearestUsableStation()
    {
        // Do a search of the current items tag
        List<ActionItemStation> usableStations = new();

        foreach (var station in FindObjectsByType<ActionItemStation>())
        {
            if (station.CompareTag(targetItem.usableStationsTag))
                usableStations.Add(station);
        }

        // Find the closest station using Linq List aggregation
        station = usableStations.Aggregate((cur, next) =>
        {
            // Check dist of best (cur) against the next list item
            if (Vector3.Distance(transform.position, cur.transform.position) < 
                Vector3.Distance(transform.position, next.transform.position))
            {
                return cur;
            }
            else
                return next;
        });
    }
}