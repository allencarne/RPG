using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    // Variables
    //public Sprite icon;
    //public new string name;
    //public string description;
    //public float attackDamage;
    //public float attackRange;
    //public float duration;
    //public float castTime;
    public int weaponIndex;

    public float leftMouse1CoolDown;
    public float leftMouse1Velocity;
    public GameObject leftMouse1Prefab;

    public float spaceCoolDown;
    public float spaceVelocity;
    public GameObject spacePrefab;

    public float projectileForce;

    // Particles

    // Sound
}
