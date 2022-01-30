using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerAimer playerAimer;

    // Start is called before the first frame update
    void Awake()
    {
        playerAimer = playerAimer.GetComponent<PlayerAimer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
