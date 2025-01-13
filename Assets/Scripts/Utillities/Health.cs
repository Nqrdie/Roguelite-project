using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health;

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;


        if (health <= 0)
        {
            Die();
        }
    }

    public void HealHealth(int healAmount)
    {
       if(health !> maxHealth)
        {
            health += healAmount;
        }

       if(healAmount + health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
