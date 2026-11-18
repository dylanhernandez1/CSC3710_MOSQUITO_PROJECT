using UnityEngine;

public class DrinkingBehavior : MonoBehaviour
{
    private BehaviorManager _manager;

    [SerializeField] private float drinkTime = 10f;
    [SerializeField] private GameObject bottle;
    [SerializeField] private string trigger = "StartDrinking";
    [SerializeField] private GameObject level2VendingMachine;
    [SerializeField] private GameObject level2FakeVial;
    [SerializeField] private GameObject level2RealVial;

    [SerializeField] private float vendingTiltAmount = 3f; // degrees
    [SerializeField] private float vendingTiltSpeed = 4f;

    private Quaternion vendingStartRotation;
    private float tiltSeed;

    private float curTime;
    private bool debounce = false;
    private Animator _animator;

    private void Awake()
    {
        curTime = drinkTime;
        _manager = GetComponent<BehaviorManager>();
        _animator = GetComponent<Animator>();
        vendingStartRotation = level2VendingMachine.transform.localRotation;
        tiltSeed = Random.value * 100f;
    }

    // Update is called once per frame
    public void Drink()
    {
        _manager.humanManager.uIManager.SetActionBubbleContent("<sprite=7>");
        TiltVendingMachine();

        if (debounce == false)
        {
            debounce = true;
            _animator.SetTrigger(trigger);
            level2FakeVial.SetActive(false);
            level2RealVial.SetActive(true);
        }

        if (curTime <= 0)
        {
            curTime = drinkTime;
            gameObject.GetComponent<BehaviorManager>().SetState("default");
            level2VendingMachine.transform.localRotation = vendingStartRotation;
            debounce = false;
        } else
        {
            curTime -= Time.deltaTime;
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Base Layer.WalkBlendTree") && !_animator.IsInTransition(0))
            {
                _animator.SetTrigger(trigger);
            }
        }
    }
    private void TiltVendingMachine()
    {
        float tilt = Mathf.Sin((Time.time + tiltSeed) * vendingTiltSpeed)
                     * vendingTiltAmount;

        level2VendingMachine.transform.localRotation =
            vendingStartRotation * Quaternion.Euler(0f, 0f, tilt);
    }
}
