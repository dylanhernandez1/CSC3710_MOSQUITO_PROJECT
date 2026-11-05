using System;
using UnityEditor;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SwitchingPlayerManager : MonoBehaviour
{
    // Conditional from absorbed liquid puddles
    private string _cond = "angry";
    [SerializeField] ParticleSystem bloodPrefab;

    [SerializeField] CinemachineBrain brain;          // on Main Camera
    [SerializeField] MonoBehaviour noclipScript;      // your noclip script, on Main Camera
    [SerializeField] FlightController flight;
    Rigidbody rigibody;
    public static event Action OnPlayerBite;

    bool noclip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNoClip(!noclip);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            SetNoClip(!noclip);
        }
    }

    void SetNoClip(bool on)
    {
        noclip = on;
        brain.enabled = !on;
        noclipScript.enabled = on;
        flight.enabled = !on;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Human"))
        {
            collision.gameObject.GetComponent<BehaviorManager>().SetState(_cond);
            Instantiate(bloodPrefab, GetComponent<Rigidbody>().transform);
            OnPlayerBite.Invoke();
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
