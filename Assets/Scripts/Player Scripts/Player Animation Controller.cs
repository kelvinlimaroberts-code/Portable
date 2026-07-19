using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    public PlayerController player;


    void Awake()
    {
        player.StateChange += stateChange;  //subscribe to state change event
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", false);
        animator.SetBool("isCrouching", false);
    }




    void stateChange()
    {
        if (player.isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
           animator.SetBool("isWalking", false);
        }

        if (player.isCrouching)
        {
            animator.SetBool("isCrouching", true);
        }
        else
        {
            animator.SetBool("isCrouching", false);
        }
    }
}
