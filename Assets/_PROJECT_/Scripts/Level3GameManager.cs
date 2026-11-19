using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class Level3GameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPasswordCorrect;
    private AudioSource _source;
    [SerializeField] private AudioClip _correct;
    [SerializeField] private AudioClip _incorrect;

    [SerializeField] private List<GameObject> lights;

    [SerializeField] private Material green;
    [SerializeField] private Material red;
    [SerializeField] private Material blue;
    [SerializeField] private Material yellow;
    [SerializeField] private Material black;
    [SerializeField] private float buttonDebounceTime = 3f;

    private bool _correctPassword = true;

    private float _nextButtonAllowedTime = 0f;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    private int _currentStep = 0;

    private bool CanPressButton()
    {
        if (Time.time < _nextButtonAllowedTime)
            return false;

        _nextButtonAllowedTime = Time.time + buttonDebounceTime;
        return true;
    }

    private void SetAllLights(Material material)
    {
        foreach (GameObject light in lights)
        {
            Renderer lightRenderer = light.GetComponent<Renderer>();

            if (lightRenderer != null)
                lightRenderer.sharedMaterial = material;
        }
    }

    private void SetCurrentLight(Material material)
    {
        if (_currentStep >= lights.Count)
            return;

        Renderer lightRenderer = lights[_currentStep].GetComponent<Renderer>();

        if (lightRenderer != null)
            lightRenderer.sharedMaterial = material;
    }
    public void PressGreen()
    {
        if (!CanPressButton())
            return;
        Debug.Log("Green button pressed");
        SetCurrentLight(green);
        CheckButton(0);
    }

    public void PressBlue()
    {
        if (!CanPressButton())
            return;
        Debug.Log("Blue button pressed");
        SetCurrentLight(blue);
        CheckButton(2);
    }

    public void PressRed()
    {
        if (!CanPressButton())
            return;
        Debug.Log("Red button pressed");
        SetCurrentLight(red);
        CheckButton(1);
    }

    public void PressYellow()
    {
        if (!CanPressButton())
            return;
        Debug.Log("Yellow button pressed");
        SetCurrentLight(yellow);
        CheckButton(3);
    }

    public void Reset()
    {
        _currentStep = 0;
        SetAllLights(black);
        _correctPassword = true;
    }

    private void CheckButton(int button)
    {
        if (button == _currentStep && _correctPassword == true)
        {

            if (_currentStep == 3)
            {
                Debug.Log("Password correct!");
                _source.PlayOneShot(_correct);
                _onPasswordCorrect?.Invoke();
                SetAllLights(green);
            }
        }
        else
        {
            _correctPassword = false;
            if (_currentStep == 3)
            {
                Debug.Log("Wrong button! Resetting password.");
                _currentStep = 0;
                _source.PlayOneShot(_incorrect);
                SetAllLights(black);
                _correctPassword = true;
                return;
            }
        }
        _currentStep++;
    }
}