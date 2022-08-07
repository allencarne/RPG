using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    //public new string name;
    public float health;
    public float maxHealth;
    public float speed;
    public float attackRange;
    public float aggroRange;

    public float attackCoolDown;

    float damage; // Temporary
    float lastAttack; // Temporary Variable that helps with Attack Cooldown
    float wanderCoolDown = 4f;
    float lastWander; // Temprary Variable that helps with Wander Cooldown
    Vector2 wayPoint; // Wander Waypoint
    float maxDistance = 5; // Wander Max Distance
    float range = 1; // Max Wander Range

    [Header("Components")]
    [HideInInspector] public EnemySpawner enemySpawner;
    //[SerializeField] EnemyScriptableObject enemyScriptableObject;
    [SerializeField] GameObject expObject;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] Rigidbody2D enemyRigidbody2D;
    [SerializeField] Transform player;
    [SerializeField] EnemyHealthbar enemyHealthbar;
    [SerializeField] GameObject hitInidcator;
    [SerializeField] Transform firePoint;

    private void Awake()
    {
        enemyHealthbar = GetComponent<EnemyHealthbar>();
        player = FindObjectOfType<Player>().transform;
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    enum EnemyState
    {
        idle,
        wander,
        chase,
        attack,
        hit,
        death
    }

    EnemyState state = EnemyState.idle;

    private void Update()
    {
        Debug.Log(state);

        switch (state)
        {   
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
                EnemyHitState(damage);
                break;
            case EnemyState.death:
                EnemyDeathtate();
                break;
        }
    }

    public void EnemyIdleState()
    {
        // Animate
        enemyAnimator.Play("Idle");
        enemyAnimator.SetFloat("Horizontal", enemyRigidbody2D.position.x);
        enemyAnimator.SetFloat("Vertical", enemyRigidbody2D.position.y);

        // Transitions
        if (Vector2.Distance(player.position, enemyRigidbody2D.position) <= aggroRange)
        {
            state = EnemyState.chase;
        }

        if (Vector2.Distance(player.position, enemyRigidbody2D.position) <= attackRange)
        {
            state = EnemyState.attack;
        }

        if (Time.time - lastWander < wanderCoolDown)
        {
            return;
        }
        lastWander = Time.time;
        state = EnemyState.wander;
    }

    public void EnemyWanderState()
    {
        // Animate
        enemyAnimator.Play("Wander");
        enemyAnimator.SetFloat("Horizontal", enemyRigidbody2D.position.x);
        enemyAnimator.SetFloat("Vertical", enemyRigidbody2D.position.y);

        transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDsetination();
        }

        // Transitions
        if (Vector2.Distance(player.position, enemyRigidbody2D.position) <= aggroRange)
        {
            state = EnemyState.chase;
        }

        if (Vector2.Distance(player.position, enemyRigidbody2D.position) <= attackRange)
        {
            state = EnemyState.attack;
        }
    }

    public void EnemyChaseState()
    {
        // Animate
        enemyAnimator.Play("Chase");

        Vector2 target = new Vector2(player.position.x, player.position.y);
        Vector2 newPos = Vector2.MoveTowards(enemyRigidbody2D.position, target, speed * Time.fixedDeltaTime);
        enemyRigidbody2D.MovePosition(newPos);

        enemyAnimator.SetFloat("Horizontal", (player.position.x - enemyRigidbody2D.position.x));
        enemyAnimator.SetFloat("Vertical", (player.position.y - enemyRigidbody2D.position.y));

        // Transitions
        if (Vector2.Distance(player.position, enemyRigidbody2D.position) >= aggroRange)
        {
            state = EnemyState.idle;
        }

        if (Vector2.Distance(player.position, enemyRigidbody2D.position) <= attackRange)
        {
            state = EnemyState.attack;
        }
    }

    public void EnemyAttackState()
    {
        if (Time.time- lastAttack < attackCoolDown)
        {
            return;
        }
        lastAttack = Time.time;

        // Animate
        enemyAnimator.Play("Attack");
        enemyAnimator.SetFloat("Horizontal", (player.position.x - enemyRigidbody2D.position.x));
        enemyAnimator.SetFloat("Vertical", (player.position.y - enemyRigidbody2D.position.y));

        Vector2 difference = player.position - transform.position;

        Instantiate(hitInidcator, firePoint.position, firePoint.rotation);
    }

    public void EnemyHitState(float damage)
    {
        // Animate
        state = EnemyState.hit;
        enemyAnimator.Play("Hit");

        health -= damage;
        enemyHealthbar.lerpTimer = 0f;

        if (health <= 0)
        {
            state = EnemyState.death;
        }
    }

    public void EnemyDeathtate()
    {
        // Animate
        enemyAnimator.Play("Death");

        // Turn off Healthbar
        enemyHealthbar.enemyUI.gameObject.SetActive(false);

        // Turn off Enemy Collider
        GetComponent<Collider2D>().enabled = false;

        // Destroy Enemy after a delay
        Destroy(gameObject, 5f);

        // Spawn another enemy
        enemySpawner.SpawnEnemy();
    }

    public void AE_AttackAnimationEnd()
    {
        state = EnemyState.idle;
    }

    public void AE_HitAnimationEnd()
    {
        state = EnemyState.idle;
    }

    public void AE_DeathAnimationEnd()
    {
        // Instantiate Exp Object
        Instantiate(expObject, transform.position, Quaternion.identity);
    }

    public void SetNewDsetination()
    {
        wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
        state = EnemyState.idle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(enemyRigidbody2D.position, aggroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyRigidbody2D.position, attackRange);
    }
}
