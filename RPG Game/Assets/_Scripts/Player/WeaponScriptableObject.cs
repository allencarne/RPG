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
    //public float animationSpeed;

    [Header("Weapon")]
    public int weaponIndex;
    public int attackDamage;

    [Header("LeftMouse1 Ability")]
    public float leftMouse1CoolDown;
    public float leftMouse1SlideVelocity;
    public GameObject leftMouse1Prefab;
    public GameObject leftMouse1HitEffect;
    public float leftMouse1KnockBackForce;
    public float leftMouse1projectileForce;

    [Header("Space Ability")]
    //public float spaceCoolDown;
    public float spaceVelocity;
    public GameObject spacePrefab;
    public float spaceprojectileForce;

    // Particles

    // Sound
}
