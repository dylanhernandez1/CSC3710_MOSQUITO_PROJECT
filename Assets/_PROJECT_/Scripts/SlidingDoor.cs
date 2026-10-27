
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [SerializeField] private Transform doorBody;
    [SerializeField] private float slideDistance = 3f;
    [SerializeField] private float slideSpeed = 2f;

    private Vector3 closedPosition;
    private bool isOpen = false;

    private void Start()
    {
        closedPosition = doorBody.localPosition;
    }

    private void Update()
    {
        if (!isOpen) return;

        Vector3 target = closedPosition + Vector3.up * slideDistance;

        doorBody.localPosition = Vector3.MoveTowards(
            doorBody.localPosition,
            target,
            slideSpeed * Time.deltaTime
        );
    }

    public void Open()
    {
        isOpen = true;
    }
}
