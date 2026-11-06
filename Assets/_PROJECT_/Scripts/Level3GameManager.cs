using Unity.VisualScripting;
using UnityEngine;

public class Level3GameManager : MonoBehaviour
{
    [SerializeField] private SlidingDoor door;
    private AudioSource _source;
    [SerializeField] private AudioClip _correct;
    [SerializeField] private AudioClip _incorrect;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    private int _currentStep = 0;

    public void PressGreen()
    {
        Debug.Log("Green button pressed");
        CheckButton(0);
    }

    public void PressBlue()
    {
        Debug.Log("Blue button pressed");
        CheckButton(2);
    }

    public void PressRed()
    {
        Debug.Log("Red button pressed");
        CheckButton(1);
    }

    public void PressYellow()
    {
        Debug.Log("Yellow button pressed");
        CheckButton(3);
    }

    private void CheckButton(int button)
    {
        if (button == _currentStep)
        {
            _currentStep++;

            Debug.Log("Correct button! Step: " + _currentStep);
            _source.PlayOneShot(_correct);

            if (_currentStep == 4)
            {
                Debug.Log("Password correct!");
                door.Open();
            }
        } else if (button < _currentStep)
        {
            Debug.Log("Previous button was pressed, not resetting password");
        }
        else
        {
            
            Debug.Log("Wrong button! Resetting password.");
            _currentStep = 0;
            _source.PlayOneShot(_incorrect);
        }
    }
}