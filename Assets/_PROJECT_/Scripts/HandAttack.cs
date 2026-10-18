using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class HandAttack : MonoBehaviour
{
    [SerializeField] private Transform player;
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
            _animator.SetTrigger("Attack");
        }
    }

    public void AttackFinished()
    {
        _isAttacking = false;
    }
}
