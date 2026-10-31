using UnityEngine;

public class Puddle : MonoBehaviour
{
    // Condition to be used in behavior manager to switch human state when injected with this liquid
    [SerializeField] private string _condition = "SampleText";
    
    public string GetCondition()
    {
        return _condition;
    }
    
}
