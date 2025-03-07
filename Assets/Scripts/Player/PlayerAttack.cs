using System.Collections;
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
        if (InventoryManager.menuActive) return;
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !collision.isTrigger)
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            StartCoroutine(enemy.Knockback());

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(35);
            }
        }
        if(collision.gameObject.CompareTag("Chest"))
        {
            Chest chest = collision.gameObject.GetComponent<Chest>();

            chest.OpenChest();
        }
    }
}
