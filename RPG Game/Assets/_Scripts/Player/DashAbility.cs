using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public float dashVelocity;

    public override void Activate(GameObject parent)
    {
        //References
        Animator animator = parent.GetComponent<Animator>();
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
        //Tranform.position
        //attackAnglePaused
        //dashMoveDistance
        //isAttacking
        //SlashPrefab
        //firePoint
        //Slashforce

        /*animator.SetTrigger("Dash");

        //Calculate the difference between mouse position and player position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (!attackAnglePaused)
        {
            //Set Attack Animation Depending on Mouse Position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            //Set Idle to last attack position
            animator.SetFloat("Horizontal", difference.x);
            animator.SetFloat("Vertical", difference.y);

            ////Slide Forward When Attacking
            //Normalize movement vector and times it by attack move distance
            difference = difference.normalized * dashMoveDistance;

            //Add force in Attack Direction
            rb.AddForce(difference, ForceMode2D.Impulse);

            //Set AttackAnglePause Bool to True
            attackAnglePaused = true;
        }

        if (isAttacking)
        {
            //Instantiate Slash prefab
            GameObject slash = Instantiate(slashPrefab, firePoint.position, firePoint.rotation);

            //Get the Rigid Body of the Slash prefab
            Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

            //Add Force to Slash prefab
            slashRB.AddForce(firePoint.up * slashForce, ForceMode2D.Impulse);

            //Reset Animator Trigger
            animator.ResetTrigger("Dash");

            //Reset isAttacking Bool;
            isAttacking = false;
        }
                */
    }
}