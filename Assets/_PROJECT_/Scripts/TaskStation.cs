using UnityEngine;
using UnityEngine.AI;

public class TaskStation : MonoBehaviour
{
    // Animation Triggers
    [SerializeField] private string _startTrigger;
    [SerializeField] private string _endTrigger;
    [SerializeField] private float _taskTime;
    [SerializeField] private Transform _location;
    
    public string GetStartTrigger()
    {
        return _startTrigger;
    }

    public string GetEndTrigger()
    {
        return _endTrigger;
    }
    public float GetTaskTime()
    {
        return _taskTime;
    }

    public Transform GetLocation()
    {
        return _location;
    }
}
