using UnityEngine;

public class DrinkingBehavior : MonoBehaviour
{
    [SerializeField] private float drinkTime = 10f;
    [SerializeField] private GameObject bottle;
    [SerializeField] private string trigger = "StartDrinking";

    private float curTime;
    private bool debounce = false;
    private Animator _animator;

    private void Awake()
    {
        curTime = drinkTime;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Drink()
    {
        if (debounce == false)
        {
            debounce = true;
            _animator.SetTrigger(trigger);
        }

        if (curTime <= 0)
        {
            curTime = drinkTime;
            gameObject.GetComponent<BehaviorManager>().SetState("default");
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
}
