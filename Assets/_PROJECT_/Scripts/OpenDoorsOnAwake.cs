using UnityEngine;

public class OpenDoorsOnAwake : MonoBehaviour
{
    [SerializeField] private ElevatorDoors _elevatorDoors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _elevatorDoors.OpenDoors();
    }
    private void Awake()
    {
        if (_elevatorDoors == null)
        {
            _elevatorDoors =
                GetComponentInParent<ElevatorDoors>();
        }
    }

}
