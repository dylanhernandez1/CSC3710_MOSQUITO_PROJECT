
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [SerializeField] private Transform doorBody;
    [SerializeField] private float slideDistance = 3f;
    [SerializeField] private float slideSpeed = 2f;
    [SerializeField] private float timeToSlide = 1f;
    [SerializeField] private AnimationCurve slideInterpolationCurve;

    private Vector3 closedPosition;
    private bool isOpen = false;
    private float timeOfOpen = float.NegativeInfinity;

    private void Start()
    {
        closedPosition = doorBody.localPosition;
    }

    private void Update()
    {
        if (!isOpen) return;

        Vector3 target = closedPosition + Vector3.up * slideDistance;

        doorBody.localPosition = Vector3.Lerp(closedPosition, target, slideInterpolationCurve.Evaluate((Time.time - timeOfOpen) / timeToSlide));
    }

    public void Open()
    {
        isOpen = true;
        timeOfOpen = Time.time;
    }
}
