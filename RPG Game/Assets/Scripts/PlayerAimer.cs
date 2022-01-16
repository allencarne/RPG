using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    public float offset;

    public Transform firePoint;
    public GameObject slashPrefab;
    public Animator animator;

    public float slashForce;

    private float attackCoolDown;
    public float startAttackCoolDown;

    void Update()
    {
        // Rotate towards mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

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
        GameObject punch = Instantiate(slashPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = punch.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * slashForce, ForceMode2D.Impulse);
        animator.SetTrigger("Attack");
    }
}
