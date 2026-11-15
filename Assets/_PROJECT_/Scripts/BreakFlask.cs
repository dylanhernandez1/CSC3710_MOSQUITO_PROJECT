using UnityEngine;

public class BreakFlask : MonoBehaviour
{
    [SerializeField] private GameObject _puddle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        _puddle.SetActive(true);
        gameObject.SetActive(false);
    }
}
