using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private float delayBeforeRandomAnimation;
    [Range(0, 1)] [SerializeField] private float chanceOfRandomAnimation;
    private float lastRandomAnimationTime;
    private Animator mcAnimator;

    private static readonly int[] IdleIndexes =
            {Animator.StringToHash("IdleCheckBottle"), 
            Animator.StringToHash("IdleLookAround")};

    private void Start()
    {
        mcAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!(Time.time - lastRandomAnimationTime > delayBeforeRandomAnimation)) return;
        lastRandomAnimationTime = Time.time;
        if(Random.value <= chanceOfRandomAnimation)
        {
            mcAnimator.SetTrigger(IdleIndexes[Random.Range(0, IdleIndexes.Length)]);
        }
    }
}
