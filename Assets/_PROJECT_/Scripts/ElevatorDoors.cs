using System.Collections;
using UnityEngine;

public class ElevatorDoors : MonoBehaviour
{
    // Fields

    [Header("References")]
    [SerializeField] private Transform _doorLeft;
    [SerializeField] private Transform _doorRight;

    [Header("Door Settings")]
    [SerializeField] private float _leftOpenDistance = 0.75f;
    [SerializeField] private float _rightOpenDistance = 0.75f;
    [SerializeField] private float _moveDuration = 0.5f;

    private Vector3 _leftClosedPosition;
    private Vector3 _rightClosedPosition;
    private Coroutine _doorCoroutine;

    // Properties

    public bool AreDoorsOpen { get; private set; }
    public bool IsMoving { get; private set; }

    // Events / Delegates

    // Monobehaviour Methods (Awake, Start, Update, ...)

    private void Awake()
    {
        if (_doorLeft != null)
        {
            _leftClosedPosition = _doorLeft.localPosition;
        }

        if (_doorRight != null)
        {
            _rightClosedPosition = _doorRight.localPosition;
        }
    }

    // Public Methods

    public void OpenDoors()
    {
        if (_doorLeft == null || _doorRight == null)
        {
            Debug.LogWarning(
                "ElevatorDoors needs Door Left and Door Right assigned."
            );
            return;
        }

        Vector3 leftOpenPosition =
            _leftClosedPosition +
            Vector3.left * _leftOpenDistance;

        Vector3 rightOpenPosition =
            _rightClosedPosition +
            Vector3.right * _rightOpenDistance;

        MoveDoors(
            leftOpenPosition,
            rightOpenPosition,
            true
        );
    }

    public void CloseDoors()
    {
        if (_doorLeft == null || _doorRight == null)
        {
            Debug.LogWarning(
                "ElevatorDoors needs Door Left and Door Right assigned."
            );
            return;
        }

        MoveDoors(
            _leftClosedPosition,
            _rightClosedPosition,
            false
        );
    }

    // Private Methods

    private void MoveDoors(
        Vector3 leftTargetPosition,
        Vector3 rightTargetPosition,
        bool willBeOpen
    )
    {
        if (_doorCoroutine != null)
        {
            StopCoroutine(_doorCoroutine);
        }

        _doorCoroutine = StartCoroutine(
            MoveDoorsRoutine(
                leftTargetPosition,
                rightTargetPosition,
                willBeOpen
            )
        );
    }

    private IEnumerator MoveDoorsRoutine(
        Vector3 leftTargetPosition,
        Vector3 rightTargetPosition,
        bool willBeOpen
    )
    {
        IsMoving = true;

        Vector3 leftStartPosition = _doorLeft.localPosition;
        Vector3 rightStartPosition = _doorRight.localPosition;

        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / _moveDuration
            );

            _doorLeft.localPosition = Vector3.Lerp(
                leftStartPosition,
                leftTargetPosition,
                progress
            );

            _doorRight.localPosition = Vector3.Lerp(
                rightStartPosition,
                rightTargetPosition,
                progress
            );

            yield return null;
        }

        _doorLeft.localPosition = leftTargetPosition;
        _doorRight.localPosition = rightTargetPosition;

        AreDoorsOpen = willBeOpen;
        _doorCoroutine = null;
        IsMoving = false;
    }
}