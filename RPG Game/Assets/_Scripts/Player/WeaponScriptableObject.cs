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
    public float attackRange;

    [Header("Basic Attack Ability")]
    public float basicAttackCoolDown;
    public float basicAttackSlideVelocity;
    public GameObject basicAttackPrefab;
    public GameObject basicAttackHitEffect;
    public float basicAttackKnockBackForce;
    public float basicAttackProjectileForce;

    [Header("Dash Ability")]
    public float dashCoolDown;
    public float dashVelocity;
    public GameObject dashPrefab;
    public float dashProjectileForce;

    // Particles

    // Sound
}
