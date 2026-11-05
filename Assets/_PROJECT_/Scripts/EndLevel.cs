using UnityEngine;

public class EndLevel : MonoBehaviour
{
    // Fields

    [SerializeField] private string _nextSceneName;

    // Properties

    public string GetNextSceneName()
    {
        return _nextSceneName;
    }
}