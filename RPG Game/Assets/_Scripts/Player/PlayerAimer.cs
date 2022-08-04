using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    Player playerScript;
    public float offset;
    public Transform firePoint;
    [SerializeField] GameObject player;
    //[SerializeField] Animator animator;
    private void Awake()
    {
        playerScript = player.GetComponent<Player>();
    }

    void Update()
    {
        // If Attack Animation is playing Don't Move the firepoint (This is to prevent moving attacks moving during animation && this dosn't handle the animation)
        if (!playerScript.attackAnglePaused)
        {
            // Rotate towards mouse position
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        }
    }
}