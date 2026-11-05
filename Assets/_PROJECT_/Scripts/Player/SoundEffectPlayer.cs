using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
[ SerializeField] private AudioSource  _audioSource;
[SerializeField] private AudioClip _biteClip;
[SerializeField] private AudioClip _doorClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        TogglePlayerManager.OnPlayerBite += HandlePlayerBite;

        SlidingDoor.OnDoorOpen += HandleDoorOpen;

    }

    private void OnDisable()
    {
        TogglePlayerManager.OnPlayerBite -= HandlePlayerBite;

        SlidingDoor.OnDoorOpen -= HandleDoorOpen;

    }

    // Update is called once per frame
    void Update()
    {
        
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
