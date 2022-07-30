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
    public float coolDown;
    public float velocity;

    public float attackMoveDistance;
    public float slashForce;

    // Particles

    // Sound
}
