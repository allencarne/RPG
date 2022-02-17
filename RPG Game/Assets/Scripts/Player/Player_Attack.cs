using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : StateMachineBehaviour
{
    Rigidbody2D rb;
    Transform transform;

    public float attackMoveDistance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get Rigidbody of Player
        rb = animator.GetComponent<Rigidbody2D>();

        // Get Transform of Player
        transform = animator.GetComponent<Rigidbody2D>().transform;

        // Change attack speed of animation
        //animator.speed = 2.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Create a Vector from Camera position subtracted by player position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Normalize movement vector and times it by attack move distance
        difference = difference.normalized * attackMoveDistance;

        // Add force in Attack Direction
        rb.AddForce(difference, ForceMode2D.Impulse);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set animation speed back to normal
        //animator.speed = 1.0f;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
