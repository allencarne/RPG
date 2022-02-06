using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform firePoint;
    public GameObject slashPrefab;
    public Animator animator;

    public float slashForce;

    private float attackCoolDown;
    public float startAttackCoolDown;

    // Update is called once per frame
    void Update()
    {
        if (attackCoolDown <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                attackCoolDown = startAttackCoolDown;
            }
        }
        else
        {
            attackCoolDown -= Time.deltaTime;
        }
    }

    void Attack()
    {
        animator.SetFloat("Aim Horizontal", firePoint.rotation.x);
        animator.SetFloat("Aim Vertical", firePoint.rotation.y);
        GameObject slash = Instantiate(slashPrefab, firePoint.position, firePoint.rotation); ;
        Rigidbody2D rb = slash.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * slashForce, ForceMode2D.Impulse);
        animator.SetTrigger("Attack");
    }
}
