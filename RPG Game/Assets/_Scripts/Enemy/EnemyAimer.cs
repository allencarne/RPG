using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimer : MonoBehaviour
{
    public float offset;
    public Transform firePoint;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate towards mouse position
        Vector3 difference = player.transform.position - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
    }
}
