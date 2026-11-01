using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Conditional from absorbed liquid puddles
    private string _cond = "angry";
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
        if (collision.gameObject.CompareTag("Human"))
        {
            collision.gameObject.GetComponent<BehaviorManager>().SetState(_cond);
            return;
        }

        Puddle puddle = collision.gameObject.GetComponent<Puddle>();
        if (puddle != null)
        {
            _cond = puddle.GetCondition();
        }
        Debug.Log("Hit " + collision.gameObject.name);
    }
}
