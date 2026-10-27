using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class HandAttack : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Collider handHitbox;
    private Animator _animator;
    private bool _isAttacking = false;
    private void Awake()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }

        _animator = GetComponent<Animator>();

        if (handHitbox != null) 
            handHitbox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void Attack()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;

            if (handHitbox != null)
                handHitbox.enabled = true;

            _animator.SetTrigger("Attack");
        }
    }

    public void AttackFinished()
    {
        _isAttacking = false;

        if (handHitbox != null)
            handHitbox.enabled = false;
    }

}
