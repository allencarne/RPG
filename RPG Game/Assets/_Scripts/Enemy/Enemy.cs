using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    //private float currentHealth;
    //[SerializeField] float maxHealth = 100;
    float damage;

    [Header("Components")]
    [SerializeField] EnemyScriptableObject enemyScriptableObject;
    //public EnemySpawner enemySpawner;
    [SerializeField] GameObject expObject;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] Rigidbody2D enemyRigidbody2D;
    [SerializeField] Transform player;
    [SerializeField] EnemyHealthbar enemyHealthbar;

    private void Awake()
    {
        enemyHealthbar = GetComponent<EnemyHealthbar>();
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

    // Start is called before the first frame update
    void Start()
    {
        // Locate Enemy spawner Class
        //enemySpawner = FindObjectOfType<EnemySpawner>();
    }

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

        if (Vector2.Distance(player.position, enemyRigidbody2D.position) <= enemyScriptableObject.attackRange)
        {
            state = EnemyState.idle;
        }

        if (Vector2.Distance(player.position, enemyRigidbody2D.position) <= enemyScriptableObject.aggroRange)
        {
            state = EnemyState.chase;
        }
    }

    public void EnemyWanderState()
    {

    }

    public void EnemyChaseState()
    {
        // Animate
        enemyAnimator.Play("Move");

        Vector2 target = new Vector2(player.position.x, player.position.y);
        Vector2 newPos = Vector2.MoveTowards(enemyRigidbody2D.position, target, enemyScriptableObject.speed * Time.fixedDeltaTime);
        enemyRigidbody2D.MovePosition(newPos);

        enemyAnimator.SetFloat("Horizontal", (player.position.x - enemyRigidbody2D.position.x));
        enemyAnimator.SetFloat("Vertical", (player.position.y - enemyRigidbody2D.position.y));

        if (Vector2.Distance(player.position, enemyRigidbody2D.position) <= enemyScriptableObject.attackRange)
        {
            state = EnemyState.idle;
        }

        if (Vector2.Distance(player.position, enemyRigidbody2D.position) <= enemyScriptableObject.attackRange)
        {
            state = EnemyState.attack;
        }
    }

    public void EnemyAttackState()
    {
        // Animate
        enemyAnimator.Play("Attack");
        enemyAnimator.SetFloat("Horizontal", (player.position.x - enemyRigidbody2D.position.x));
        enemyAnimator.SetFloat("Vertical", (player.position.y - enemyRigidbody2D.position.y));
    }

    public void EnemyHitState(float damage)
    {
        // Animate
        state = EnemyState.hit;
        enemyAnimator.Play("Hit");

        enemyScriptableObject.health -= damage;
        enemyHealthbar.lerpTimer = 0f;

        if (enemyScriptableObject.health <= 0)
        {
            state = EnemyState.death;
        }
    }

    public void EnemyDeathtate()
    {
        // Animate
        enemyAnimator.Play("Death");

        // Turn off Healthbar
        //enemyUI.gameObject.SetActive(false);

        // Turn off Enemy Collider
        GetComponent<Collider2D>().enabled = false;

        // Destroy Enemy after a delay
        Destroy(gameObject, 5f);

        // Spawn another enemy
        //enemySpawner.SpawnEnemy();
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
}
