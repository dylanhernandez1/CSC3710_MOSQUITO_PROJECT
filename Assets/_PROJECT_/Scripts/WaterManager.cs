using UnityEngine;

public class WaterManager : MonoBehaviour
{
    //private bool _debounce = false;
    [SerializeField] private GameObject _flask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (_flask == null)
        {
            Debug.LogError("Flask not given to water manager in water dispensor");
        }
        //_debounce = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Human"))
            return;

        //_debounce = true;
        //_flask.SetActive(true);
        collision.gameObject.GetComponent<BehaviorManager>().SetState("slip");

    }

    public void GenerateFlask()
    {
        _flask.SetActive(true);
    }
}
