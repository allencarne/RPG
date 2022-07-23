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
}
