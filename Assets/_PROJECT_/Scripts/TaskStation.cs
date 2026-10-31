using UnityEngine;
using UnityEngine.AI;

public class TaskStation : MonoBehaviour
{
    // Animation Triggers
    [SerializeField] private string _startTrigger;
    [SerializeField] private string _endTrigger;

    // Time a task should take
    [SerializeField] private float _taskTime = 30f;
    // Location/game object where the human should perform its animations
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
