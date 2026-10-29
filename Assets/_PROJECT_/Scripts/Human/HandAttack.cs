using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Collider handHitbox;
    private Animator _animator;
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Transform _handTargetTransform;
    [SerializeField] private Transform _slapStartPosTransform;
    [SerializeField] private TwoBoneIKConstraint _twoBoneIKConstraint;
    [Space]
    [SerializeField] private Transform _torsoRotationTarget;
    [SerializeField] private MultiRotationConstraint _torsoRotationConstraint;

    [Space(10)]
    private FollowBehavior _followBehavior;

    [Header("Settings")]
    [SerializeField] private float _timeToRaiseHand = .7f;
    [SerializeField] private float _timeToReachTarget;
    [SerializeField] private AnimationCurve _slapLerpCurve;
    [SerializeField] private float _timeToHold;
    [SerializeField] private float _timeToPullBack;
    [SerializeField] private AnimationCurve _pullBackLerpCurve;


    [Header("Local State")]
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
        _followBehavior = GetComponent<FollowBehavior>();

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

            // Begin a procedural attack 
            // OLD: _animator.SetTrigger("Attack");

            Debug.Log("Running Slap");
            StartCoroutine(HandSlap(player.position));
        }
    }

    public void AttackFinished()
    {
        _isAttacking = false;

        if (handHitbox != null)
            handHitbox.enabled = false;
    }

    IEnumerator HandSlap(Vector3 slapTargetPosition)
    {  
        // Disable movement
        _followBehavior.disableMovement = true;

        Vector3 startingHandTargetPosition = _handTargetTransform.position;

        // TODO: Kinda a sloppy, lazy, and wasteful way to handle the timers. Needs refactoring - TH

        // Calculate torso rotation
        // y = 7.5 is around about the center of the human
        float bendPercent = (7.5f - Mathf.Clamp(slapTargetPosition.y, 0, 7.5f)) / 7.5f;
        float desiredTorsoXRot = bendPercent * 90;
        float startingTorsoXRot = _torsoRotationTarget.localRotation.x;

        // Transfer animation to IK and move to slap pos
        // Rotate torso as needed
        float timeOfStart = Time.time;
        while (Time.time - timeOfStart <= _timeToRaiseHand)
        {
            float t = (Time.time - timeOfStart) / _timeToRaiseHand;
            
            // Hand
            _twoBoneIKConstraint.weight = t;
            _handTargetTransform.position = Vector3.Lerp(startingHandTargetPosition, _slapStartPosTransform.position, t);

            // Torso
            _torsoRotationConstraint.weight = t;
            _torsoRotationTarget.localRotation = Quaternion.Euler(Mathf.Lerp(startingTorsoXRot, desiredTorsoXRot, t), 0, 0);

            yield return null;
        }
        // Make sure the torso and IK constraints are fully on
        /// This means that the original walk animation has NO
        /// effect on the human rig.
        _twoBoneIKConstraint.weight = 1;
        _torsoRotationConstraint.weight = 1;


        // Slap Logic
        Vector3 positionAtStartOfSlap = _handTargetTransform.position;
        float timeOfSlap = Time.time;
        while (Time.time - timeOfSlap <= _timeToReachTarget)
        {
            float t = (Time.time - timeOfSlap) / _timeToReachTarget;

            _handTargetTransform.position = Vector3.Lerp(positionAtStartOfSlap, slapTargetPosition, _slapLerpCurve.Evaluate(t));

            yield return null;
        }

        // Hold Slap
        yield return new WaitForSeconds(_timeToHold);


        // Return to original pos / release IK weight
        timeOfStart = Time.time;
        while (Time.time - timeOfStart <= _timeToPullBack)
        {
            float t = (Time.time - timeOfStart) / _timeToPullBack;

            // Head
            _twoBoneIKConstraint.weight = 1 - t;
            _handTargetTransform.position = Vector3.Lerp(
                                                slapTargetPosition, 
                                                _slapStartPosTransform.position, 
                                                _pullBackLerpCurve.Evaluate(t)
                                            );

            // Torso
            _torsoRotationConstraint.weight = 1 - t;
            _torsoRotationTarget.localRotation = Quaternion.Euler(Mathf.Lerp(desiredTorsoXRot, startingTorsoXRot, t), 0, 0);

            yield return null;
        }

        _twoBoneIKConstraint.weight = 0;

        // Make sure torso is reset
        _torsoRotationConstraint.weight = 0;
        _torsoRotationTarget.localRotation = Quaternion.Euler(startingTorsoXRot, 0, 0);
        

        // Re-enable follow logic
        _followBehavior.disableMovement = false;
        _isAttacking = false;
    }
}
