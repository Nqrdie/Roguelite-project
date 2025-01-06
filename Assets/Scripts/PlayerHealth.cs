using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable<int>
{
    private int health = 100;

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;


        if (health < 0)
        {
            Die();
        }
    }

    public void Damagetake(int damageAmount)
    {
        health -= damageAmount;


        if (health < 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Destroy(gameObject);
    }
}
