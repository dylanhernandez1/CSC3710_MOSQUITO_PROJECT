using System.Runtime.CompilerServices;
using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class HumanManager : MonoBehaviour
{
    public BehaviorManager behaviorManager;
    
    public HumanUIManager uIManager;

    public ActionItemBehavior actionItemBehavior;
    
    [SerializeField] private GameObject _level4splat;

    void OnCollisionEnter(Collision collision)
    {
        CreateSplat(collision);
    }

    void CreateSplat(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") || _level4splat == null)
        {
            return;
        }

        if (GetComponent<TaskManager>().GetTask() != 0)
        {
            return;
        }

        Collider[] hits = Physics.OverlapSphere(
            collision.transform.position,
            0.5f,
            ~0,
            QueryTriggerInteraction.Collide // include trigger colliders
        );

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("TaskStation"))
                continue;

            _level4splat.SetActive(true);
            return;
        }
    }
}

