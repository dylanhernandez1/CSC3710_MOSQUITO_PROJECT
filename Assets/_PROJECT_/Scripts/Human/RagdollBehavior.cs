using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class RagdollBehavior : MonoBehaviour
{
    // Terribly ugly temp implementation of refs, will refactor soon
    private HumanUIManager _uiMan;

    private Animator _animator;
    private NavMeshAgent _agent;
    private CapsuleCollider _capsule;
    private Rigidbody[] _bodies;
    private bool _first = true;

    [SerializeField] private UnityEvent _trigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _bodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _capsule = GetComponent<CapsuleCollider>();
        _uiMan = GetComponent<HumanUIManager>();
    }

    public void SetRagdoll(bool enabled)
    {
        _animator.enabled = !enabled;
        _agent.enabled = !enabled;
        _capsule.enabled = !enabled;

        foreach (Rigidbody body in _bodies)
        {
            body.isKinematic = !enabled;
            body.detectCollisions = enabled;
        }
    }

    // Update is called once per frame
    public void Die()
    {
        _uiMan.SetActionBubbleContent("<sprite=6>");
        
        if (_first != true)
        {
            return;
        }
        _first = false;
        SetRagdoll(true);
        if(_trigger != null)
        {
            _trigger.Invoke();
        }
    }
}
