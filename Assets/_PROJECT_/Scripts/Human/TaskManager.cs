using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/*
 * HOW TO USE: this can all be done from the inspector tab
 * 
 * Step 1: Add an animation state + trigger in the human's animator with a transition from Any State
 * Step 2: Create a work station, this requires a game object with the TaskStation script + a trigger hitbox that should envelop the human while working
 * Step 3: Fill the TaskStation script with the name of the animation trigger and the location where the animation will be performed. This could be a game object
 * Step 4: Add this script to the human + assign its ExecuteTasks() function as the default behavior under behavior manager
 * Step 5: Add work stations to _stations by clicking the '+' button and then dragging/dropping
 * Step 6: Profit
 */

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskStation> _stations = new();
    [SerializeField] private float _standingDistance = 0f;
    private int _curStation = 0;
    private float _remainingTaskTime = 10;


    private NavMeshAgent _agent;
    private Animator _animator;
    private BehaviorManager _manager;
    private bool _debounce = false;

    private bool _init = false;
    private int _behaveState = 0;
    //private bool _endingAnimation = false;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _manager = GetComponent<BehaviorManager>();
        if (_stations.Count != 0)
        {
            _remainingTaskTime = _stations[_curStation].GetTaskTime();
        }
    }



    // Update is called once per frame
    public void ExecuteTasks()
    {
        if (_init == false)
        {
            _behaveState = _manager.GetState();
            _init = true;
        }

        if (_stations.Count == 0)
        {
            Debug.LogError("Empty Task List");
        }

        if (_remainingTaskTime <= 0)
        {
            if (_stations.Count != 1)
            {
                //string endTrigger = _stations[_curStation].GetEndTrigger();
                string endTrigger = null;
                if (endTrigger != null)
                {
                    _animator.SetTrigger(endTrigger);
                    //StartCoroutine(EndAnimation());
                }
                else
                {
                    _animator.SetTrigger("Walk");
                }
            }

            if (_stations.Count == _curStation + 1)
            {
                _curStation = 0;
                
            } else
            {
                _curStation++;
            }
            _remainingTaskTime = _stations[_curStation].GetTaskTime();
            _agent.isStopped = false;
            _agent.updateRotation = true;
        }

        float distance = Vector3.Distance(transform.position, _stations[_curStation].GetLocation().position);
        if (distance > _standingDistance)
        {
            _agent.isStopped = false;
            _agent.updateRotation = true;
            _agent.SetDestination(_stations[_curStation].GetLocation().position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == _stations[_curStation].gameObject)
        {
            _remainingTaskTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_debounce == true)
        {
            return;
        }

        if (other.gameObject == _stations[_curStation].gameObject && _manager.GetState() == _behaveState)
        {
            StartCoroutine(Debounce());
        }
    }

    IEnumerator Debounce()
    {
        _debounce = true;

        _agent.isStopped = true;
        _agent.updateRotation = false;
        _animator.SetTrigger(_stations[_curStation].GetStartTrigger());
        transform.rotation = _stations[_curStation].GetLocation().rotation;
        transform.position = _stations[_curStation].GetLocation().position;

        yield return new WaitForSeconds(4f);

        _debounce = false;
    }

    //IEnumerator EndAnimation()
    //{

    //}

    public int GetTask()
    {
        return _curStation;
    }
}
