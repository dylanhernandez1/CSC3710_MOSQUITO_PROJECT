using UnityEngine;
using UnityEngine.InputSystem;

public class SoundEffectsPlayer : MonoBehaviour
{
    // Fields
    [SerializeField] private GameObject _audioSourceGameObject;
    [SerializeField] private AudioClip _biteClip;
    //[SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _doorClip;
    //[SerializeField] private AudioClip _starClip;

    private AudioSource _audioSource;
    private void Start()
    {
        _audioSource = _audioSourceGameObject.GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        PlayerManager.OnPlayerBite += HandlePlayerBite;
        SwitchingPlayerManager.OnPlayerBite += HandlePlayerBite;
        SlidingDoor.OnDoorOpen += HandleDoorOpen;
        
    }

    private void OnDisable()
    {


        PlayerManager.OnPlayerBite -= HandlePlayerBite;
        SwitchingPlayerManager.OnPlayerBite -= HandlePlayerBite;

        SlidingDoor.OnDoorOpen -= HandleDoorOpen;

    }


  




    private void HandlePlayerBite()
    {
        _audioSource.PlayOneShot(_biteClip);
    }

    private void HandleDoorOpen()
    {
        _audioSource.PlayOneShot(_doorClip);
    }







}