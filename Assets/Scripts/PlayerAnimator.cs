using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        animator.SetBool("Walking", playerMovement.isWalking);
        animator.SetBool("Sprinting", playerMovement.isSprinting);
        animator.SetBool("WalkingBackwards", playerMovement.isWalkingBackward);
        animator.SetFloat("Strafing", playerMovement.isStrafing ? playerMovement.movementDirection : 0);
        animator.SetBool("Attacking", playerAttack.isAttacking);

    }
}
