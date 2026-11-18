using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorLevelExit : MonoBehaviour
{
    // Fields

    [Header("References")]
    [SerializeField] private ElevatorDoors _elevatorDoors;

    [Header("Level Exit")]
    [SerializeField] private string _nextSceneName;
    [SerializeField] private float _waitAfterDoorsClose = 1.5f;

    private bool _isLeaving;

    // Properties

    // Events / Delegates

    // Monobehaviour Methods (Awake, Start, Update, ...)

    private void Awake()
    {
        if (_elevatorDoors == null)
        {
            _elevatorDoors =
                GetComponentInParent<ElevatorDoors>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isLeaving || !other.CompareTag("Player"))
        {
            return;
        }

        _isLeaving = true;
        StartCoroutine(LeaveLevelRoutine());
    }

    // Public Methods

    // Private Methods

    private IEnumerator LeaveLevelRoutine()
    {
        if (_elevatorDoors != null)
        {
            _elevatorDoors.CloseDoors();

            yield return new WaitUntil(
                () => !_elevatorDoors.IsMoving
            );
        }

        yield return new WaitForSeconds(
            _waitAfterDoorsClose
        );

        SceneManager.LoadScene(_nextSceneName);
    }
}