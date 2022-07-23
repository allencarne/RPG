using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    //Variables
    [SerializeField] private float attackRange;
    [SerializeField] private float aggroRange;

    //Components
    [SerializeField] GameObject player;
    [SerializeField] Animator animator;

    [HideInInspector] Transform playerTransform;
    [HideInInspector] Rigidbody2D playerRigidBody;

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
        playerTransform = player.GetComponent<Transform>();
        playerRigidBody = player.GetComponent<Rigidbody2D>();
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
        state = EnemyState.idle;
    }

    public void EnemyIdleState()
    {
        //Set animation based on Horz and Vert Positions
        animator.SetFloat("Horizontal", playerRigidBody.position.x);
        animator.SetFloat("Vertical", playerRigidBody.position.y);

        // If Player is in range - Attack
        if (Vector2.Distance(playerTransform.position, playerRigidBody.position) <= attackRange)
        {
            //animator.SetTrigger("Attack");
            state = EnemyState.attack;
        }

        //If Player is in range - Chase
        if (Vector2.Distance(playerTransform.position, playerRigidBody.position) <= aggroRange)
        {
            //animator.SetTrigger("Chase");
            state = EnemyState.chase;
        }
    }

    public void EnemyWanderState()
    {

    }

    public void EnemyChaseState()
    {

    }

    public void EnemyAttackState()
    {

    }

    public void EnemyHitState()
    {

    }

    public void EnemyDeathState()
    {

    }
}
