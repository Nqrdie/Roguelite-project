using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float aggroRange;

    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackDuration;

    [SerializeField] private float attackRange;
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Collider hitCollider;
    private bool isAttacking = false;
    private float lastAttack;

    protected States currentState;
    protected States previousState;

    private Transform playerTransform;
    private Transform enemyTransform;

    private NavMeshAgent enemyAgent;

    protected enum States
    {
        idle,
        chasing,
        blocking,
        running,
        knockback,
        attacking,
    }

    protected void Start()
    {
        currentState = States.idle;
        enemyTransform = transform;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAgent = GetComponent<NavMeshAgent>();

        SwitchStates(currentState);

    }

    private void Update()
    {
        HandleStates();

        if(playerTransform == null)
        {
            Destroy(gameObject.GetComponent<Enemy>());
        }
    }

    private void SwitchStates(States newState)
    {
        previousState = currentState;
        currentState = newState;
    }

    protected void HandleStates()
    {
        switch (currentState)
        {
            case States.idle:
                Idle();
                break;
            case States.chasing:
                Chasing();
                break;
            case States.attacking:
                StartCoroutine(Attacking());
                break;
        }
    }

    protected void Idle()
    {
        if(Vector3.Distance(enemyTransform.position, playerTransform.position) < aggroRange)
        {
            SwitchStates(States.chasing);
        }
    }

    protected void Chasing()
    {
        // Chase the player
        if(enemyAgent.enabled)
            enemyAgent.SetDestination(playerTransform.position);


        // Switch to Patrolling/Attacking based on distance to player
        if (Vector3.Distance(enemyTransform.position, playerTransform.position) < 1)
        {
           SwitchStates(States.attacking);
        }
    }

    protected IEnumerator Attacking()
    {
        // Switch to previous state based on distance to player
        if (Vector3.Distance(enemyTransform.position, playerTransform.position) > attackRange)
        {
            SwitchStates(previousState);
        }

        // Attack the player
        if (Time.time >= lastAttack + attackCooldown && !isAttacking)
        {
            isAttacking = true;

            // Give the animation time to wind up
            yield return new WaitForSeconds(0.1f);

            // Enable the collider for hit registration
            hitCollider.enabled = true;

            yield return new WaitForSeconds(attackDuration);

            hitCollider.enabled = false;
            isAttacking = false;

            lastAttack = Time.time;
        }
    }

    public IEnumerator Knockback()
    {
        SwitchStates(States.knockback);
        Vector3 direction = (enemyTransform.position - playerTransform.position).normalized;
        float elapsedTime = 0f;

        enemyAgent.enabled = false;

        while (elapsedTime < knockbackDuration)
        {

            gameObject.GetComponent<Rigidbody>().AddForce(direction * knockbackForce * Time.deltaTime, ForceMode.Force);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyAgent.enabled = true;

        SwitchStates(previousState);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger)
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(35);
            }
        }
    }
}
