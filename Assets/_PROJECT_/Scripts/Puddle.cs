using UnityEngine;

public class Puddle : MonoBehaviour
{
    // Condition to be used in behavior manager to switch human state when injected with this liquid
    [SerializeField] private string _condition = "SampleText";

    public bool isPheromone = false; // Is a pheromone
    public string currentEffectActionBubbleText = "";
    public Color pheromoneColor;

    public string GetCondition()
    {
        return _condition;
    }
    
}
