using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections;

public class SwitchManager : MonoBehaviour
{
    private float _rotation;
    private Transform _transform;

    private bool _isWaiting;

    public static event Action OnSwitchFlipped;

    [SerializeField] GameObject _thingTurningOff;

    [SerializeField] GameObject _switchFlipper;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
        _rotation = _transform.rotation.z;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isWaiting == false)
        {
            print(collision.gameObject.tag);
            if (collision.gameObject.CompareTag("Untagged"))
            {
                SwitchFlip();

            }
            StartCoroutine(WaitAndRun());
        }
    }

    private void SwitchFlip()
    {
        //OnSwitchFlipped.Invoke();
        transform.Rotate(0, 0, 90);
        _rotation = -_rotation;
        if (_thingTurningOff.activeSelf)
        {
            _thingTurningOff.SetActive(false);
            
        }
        else
        {
            _thingTurningOff.SetActive(true);
        }
        
    }

    IEnumerator WaitAndRun()
    {
        _isWaiting = true;
        Debug.Log("Condition met! Waiting for 2 seconds...");

        // Pause here for 2 seconds
        yield return new WaitForSeconds(2f);

        Debug.Log("2 seconds have passed! Running the rest of the code.");
        _isWaiting = false;
    }
}


    

