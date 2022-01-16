using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    public float offset;

    public Transform firePoint;
    public GameObject punchPrefab;

    public float punchForce;

    private float shootCoolDown;
    public float startShootCoolDown;

    void Update()
    {
        // Rotate towards mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (shootCoolDown <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
                shootCoolDown = startShootCoolDown;
            }
        }
        else
        {
            shootCoolDown -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        GameObject punch = Instantiate(punchPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = punch.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * punchForce, ForceMode2D.Impulse);
    }
}
