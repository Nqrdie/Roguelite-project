using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float aggroRange;

    [SerializeField] protected float knockbackForce;
    [SerializeField] protected float knockbackDuration;

    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDuration;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected Collider hitCollider;
    private bool isAttacking = false;
    private float lastAttack;

    protected States currentState;
    protected States previousState;

    protected Transform playerTransform;
    protected Transform enemyTransform;

    protected NavMeshAgent enemyAgent;

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
        if (Vector3.Distance(enemyTransform.position, playerTransform.position) < attackRange)
        {
           SwitchStates(States.attacking);
        }
    }

    protected virtual IEnumerator Attacking()
    {
        // Switch to previous state based on distance to player
        if (Vector3.Distance(enemyTransform.position, playerTransform.position) > attackRange)
        {
            SwitchStates(States.chasing);
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
        Rigidbody rb = GetComponent<Rigidbody>();
        if(currentState != States.knockback)
            SwitchStates(States.knockback);

        Vector3 direction = (enemyTransform.position - playerTransform.position).normalized;

        enemyAgent.enabled = false;

        rb.AddForce(direction * knockbackForce * Time.deltaTime, ForceMode.Impulse);
        
        yield return new WaitForSeconds(knockbackDuration);

        enemyAgent.enabled = true;
        rb.velocity = Vector3.zero;

        SwitchStates(States.chasing);
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
