using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    [SerializeField] Animator animator;

    enum DummyState
    {
        idle,
        hit
    }

    DummyState state = DummyState.idle;

    void Update()
    {
        switch (state)
        {
            case DummyState.idle:
                TrainingDummyIdleState();
                break;
            case DummyState.hit:
                TrainingDummyHitState();
                break;
        }
    }

    public void TrainingDummyIdleState()
    {
        //state = DummyState.idle;
        animator.Play("TrainingDummyIdle");
    }

    public void TrainingDummyHitState()
    {
        state = DummyState.hit;
        animator.Play("TrainingDummyHit");
    }

    public void AE_TrainingDummyHitStateEnd()
    {
        state = DummyState.idle;
    }
}
