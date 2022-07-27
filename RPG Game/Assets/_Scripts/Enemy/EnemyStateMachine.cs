using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    //Variables
    [SerializeField] private float attackRange;
    [SerializeField] private float aggroRange;
    [SerializeField] private float enemySpeed;

    //Components
    [SerializeField] GameObject player;
    [SerializeField] Transform playerTransform;
    [HideInInspector] Rigidbody2D playerRigidBody;
    [HideInInspector] Rigidbody2D enemyRigidBody;
    [HideInInspector] Animator animator;

    Enemy enemy;
    [SerializeField] GameObject enemyObject;

    enum EnemyState
    {
        spawn,
        idle,
        wander,
        chase,
        attack,
        hit,
        death
    }

    EnemyState state = EnemyState.spawn;

    private void Awake()
    {
        enemy = enemyObject.GetComponent<Enemy>();
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(state);

        switch (state)
        {
            case EnemyState.spawn:
                EnemySpawnState();
                break;
            case EnemyState.idle:
                EnemyIdleState();
                break;
            case EnemyState.wander:
                EnemyWanderState();
                break;
            case EnemyState.chase:
                EnemyChaseState();
                break;
            case EnemyState.attack:
                EnemyAttackState();
                break;
            case EnemyState.hit:
                EnemyHitState();
                break;
            case EnemyState.death:
                EnemyDeathState();
                break;
        }
    }

    public void EnemySpawnState()
    {
        //IdleAfterSpawn Animation Event
    }

    public void EnemyIdleState()
    {
        //Set animation based on Horz and Vert Positions
        animator.SetFloat("Horizontal", playerRigidBody.position.x);
        animator.SetFloat("Vertical", playerRigidBody.position.y);

        //If Player is in range - Chase
        if (Vector2.Distance(playerTransform.position, enemyRigidBody.position) <= aggroRange)
        {
            animator.SetBool("isChasing", true);
            state = EnemyState.chase;
        }
    }

    public void EnemyWanderState()
    {

    }

    public void EnemyChaseState()
    {
        //Get target position and move to that position
        Vector2 target = new Vector2(playerRigidBody.position.x, playerRigidBody.position.y);
        Vector2 newPos = Vector2.MoveTowards(enemyRigidBody.position, target, enemySpeed * Time.fixedDeltaTime);
        enemyRigidBody.MovePosition(newPos);

        //Animate
        animator.SetFloat("Horizontal", (playerRigidBody.position.x - enemyRigidBody.position.x));
        animator.SetFloat("Vertical", (playerRigidBody.position.y - enemyRigidBody.position.y));

        //If Player is out of AggroRange - Idle
        if (Vector2.Distance(playerTransform.position, enemyRigidBody.position) >= aggroRange)
        {
            animator.SetBool("isChasing", false);
            state = EnemyState.idle;
        }

        // If Player is in range - Attack
        if (Vector2.Distance(playerTransform.position, enemyRigidBody.position) <= attackRange)
        {
            animator.SetBool("isChasing", false);
            animator.SetTrigger("Attack");
            state = EnemyState.attack;
        }
    }

    public void EnemyAttackState()
    {
        //IdleAfterAttack Animation Event

        //Animate
        //animator.SetFloat("Horizontal", (playerTransform.position.x - enemyRigidBody.position.x));
        //animator.SetFloat("Vertical", (playerTransform.position.y - enemyRigidBody.position.y));
    }

    public void EnemyHitState()
    {

    }

    public void EnemyDeathState()
    {
    }

    //Animation Events
    public void IdleAfterSpawn()
    {
        state = EnemyState.idle;
    }

    public void IdleAfterAttack()
    {
        state = EnemyState.idle;
    }

    //Other Methods
    public void TakeDamage(float damage)
    {
        enemy.currentHealth -= damage;
        enemy.lerpTimer = 0f;

        // Player hurt animation
        animator.SetTrigger("Hurt");

        if (enemy.currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        // Die animation
        animator.SetTrigger("Dead");

        // Instantiate Exp Object
        Instantiate(enemy.expObject, transform.position, Quaternion.identity);

        // Turn off Healthbar
        enemy.enemyUI.gameObject.SetActive(false);

        // Turn off Enemy Collider
        GetComponent<Collider2D>().enabled = false;

        // Destroy Enemy after a delay
        Destroy(gameObject, 5f);

        // Spawn another enemy
        enemy.enemySpawner.SpawnEnemy();
    }

    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyRigidBody.position, aggroRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemyRigidBody.position, attackRange);
    }
    */
}
