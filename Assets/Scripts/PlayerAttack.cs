using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private InputHandler input;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Collider hitCollider;

    public bool isAttacking;
    private float lastAttack;

    private void Awake()
    {
        input = FindObjectOfType<InputHandler>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(input.attackTriggered)
        {
            if(Time.time >= lastAttack + attackCooldown)
            {
                StartCoroutine(Attack());
                lastAttack = Time.time;
            }
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        // Give the animation time to wind up
        yield return new WaitForSeconds(0.1f); 

        // Enable the collider for hit registration
        hitCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);
        hitCollider.enabled = false; 
        isAttacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamageable<int> enemyHealth = collision.gameObject.GetComponent<IDamageable<int>>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(35);
            }
        }
    }
}
