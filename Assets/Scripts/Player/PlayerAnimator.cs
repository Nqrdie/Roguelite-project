using UnityEngine;
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    public void AnimUpdate()
    {
        animator.SetBool("Walking", playerMovement.isWalking);
        animator.SetBool("Sprinting", playerMovement.isSprinting);
        animator.SetBool("WalkingBackwards", playerMovement.isWalkingBackward);
        animator.SetFloat("Strafing", playerMovement.isStrafing ? playerMovement.movementDirection : 0);
        animator.SetBool("Attacking", playerAttack.isAttacking);

    }
}
