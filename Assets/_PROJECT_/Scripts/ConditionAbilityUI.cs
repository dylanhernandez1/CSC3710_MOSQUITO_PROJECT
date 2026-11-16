using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConditionAbilityUI : MonoBehaviour
{
    // Fields

    [Header("References")]
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private Image _abilityImage;
    [SerializeField] private TextMeshProUGUI _abilityText;

    [Header("Ability Sprites")]
    [SerializeField] private Sprite _emptyAbilitySprite;
    [SerializeField] private Sprite _spicyAbilitySprite;
    [SerializeField] private Sprite _sleepAbilitySprite;

    // Monobehaviour Methods (Awake, Start, Update, ...)

    private void OnEnable()
    {
        if (_playerManager != null)
        {
            _playerManager.OnConditionChanged += UpdateAbilityDisplay;
        }
    }

    private void Start()
    {
        if (_playerManager != null)
        {
            UpdateAbilityDisplay(_playerManager.CurrentCondition);
        }
    }

    private void OnDisable()
    {
        if (_playerManager != null)
        {
            _playerManager.OnConditionChanged -= UpdateAbilityDisplay;
        }
    }

    // Private Methods

    private void UpdateAbilityDisplay(string condition)
    {
        if (_abilityImage == null)
        {
            return;
        }

        string normalizedCondition = condition.ToLowerInvariant();

        _abilityImage.sprite = _emptyAbilitySprite;

        switch (normalizedCondition)
        {
            case "spicy":
                _abilityImage.sprite = _spicyAbilitySprite;
                _abilityText.text = "Spicy";
            break;

            case "sleep":
                _abilityImage.sprite = _sleepAbilitySprite;
                _abilityText.text = "Sleep";
            break;

            case "angry":
                _abilityImage.sprite = _emptyAbilitySprite;
                _abilityText.text = "angry";
            break;

            default:
                _abilityImage.sprite = _emptyAbilitySprite;
                _abilityText.text = "angry";
            break;
        }

        _abilityImage.preserveAspect = true;
        _abilityImage.color = Color.white;
    }
}