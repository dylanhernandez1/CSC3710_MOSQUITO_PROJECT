using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    // Fields

    [SerializeField] private string _nextSceneName;

    // Properties

    public string GetNextSceneName()
    {
        return _nextSceneName;
    }
}