using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    }

    public void EnemyIdleState()
    {

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
