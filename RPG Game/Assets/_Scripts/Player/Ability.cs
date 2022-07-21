using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public float coolDownTime;
    public float activeTime;
    // Cooldown // Duration // CastTime // Icon // Description // Cost (Mana,Energy) // Requires Target?

    // Particle and Sound
    
    // List of Ability Behaviours

    public virtual void Activate(GameObject parent)
    {

    }
}