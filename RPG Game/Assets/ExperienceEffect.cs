using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceEffect : MonoBehaviour
{
    Transform target;

    [SerializeField] private float speed;

    void Start ()
    {
        target = GameObject.Find("Player Aimer").transform;
    }

    private void FixedUpdate()
    {
        // Move EXP to Target
        Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.Lerp(a, b, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<LevelSystem>().GainExperience(20f);
            Destroy(gameObject);
        }
    }
}
