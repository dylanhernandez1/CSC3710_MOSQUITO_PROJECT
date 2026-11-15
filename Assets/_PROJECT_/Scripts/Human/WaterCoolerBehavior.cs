using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class WaterCoolerBehavior : MonoBehaviour
{
    // Terribly ugly temp implementation of refs, will refactor soon
    private HumanUIManager _uiMan;

    private Animator _animator;
    private NavMeshAgent _agent;
    private bool _first = true;
    [SerializeField] private float drinkTime = 10f;
    private BehaviorManager _manager;
    private float curTime;
    private bool debounce = false;
    [SerializeField] private string trigger = "StartDrinking";


    [SerializeField] private GameObject _water1;
    [SerializeField] private GameObject _water2;
    [SerializeField] private Transform _coolerLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _uiMan = GetComponent<HumanUIManager>();
        _manager = GetComponent<BehaviorManager>();
        curTime = drinkTime;
    }

    // Update is called once per frame
    public void WalkToCooler()
    {
        _uiMan.SetActionBubbleContent("<sprite=7>");

        _agent.SetDestination(_coolerLocation.position);

        if (curTime <= 0)
        {
            curTime = drinkTime;
            gameObject.GetComponent<BehaviorManager>().SetState("default");
            _water1.SetActive(true);
            _water2.SetActive(true);
            debounce = false;
            _first = false;
        }
        else if (debounce)
        {
            curTime -= Time.deltaTime;
        }

        if (_first != true)
        {
            return;
        }
        _first = false;
        _animator.SetTrigger("Walk");
        _agent.isStopped = false;
        _agent.updateRotation = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("TaskStation") || _first)
        {
            return;
        }
        
        if (debounce == false)
        {
            _water1.SetActive(true);
            _water2.SetActive(true);
            _animator.SetTrigger(trigger);
            debounce = true;
        }


    }
}
