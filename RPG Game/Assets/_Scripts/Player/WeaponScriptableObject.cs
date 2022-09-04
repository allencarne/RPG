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

    [Header("Basic Attack")]
    public GameObject basicAttackPrefab;
    public GameObject basicAttackHitEffect;
    public float basicAttackCoolDown;
    public float basicAttackSlideVelocity;
    public float basicAttackKnockBackForce;
    public float basicAttackProjectileForce;

    [Header("Basic Attack 2")]
    public GameObject basicAttack2Prefab;
    //public float basicAttack2CoolDown;
    //public float basicAttack2SlideVelocity;
    //public GameObject basicAttack2HitEffect;
    //public float basicAttack2KnockBackForce;
    //public float basicAttack2ProjectileForce;

    [Header("Basic Attack 3")]
    public GameObject basicAttack3Prefab;
    //public float basicAttack3CoolDown;
    //public float basicAttack3SlideVelocity;
    //public GameObject basicAttack3HitEffect;
    //public float basicAttack3KnockBackForce;
    //public float basicAttack3ProjectileForce;

    [Header("Ability 1")]
    public GameObject ability1Prefab;
    public GameObject ability1BasePrefab;
    public float ability1CoolDown;
    public float ability1ProjectileForce;
    public float ability1KnockBackForce;

    [Header("Dash Ability")]
    public GameObject dashPrefab;
    public GameObject dashParticle;
    public float dashCoolDown;
    public float dashVelocity;
    public float dashProjectileForce;

    [Header("Ability2")]
    public GameObject ability2Prefab;
    //public GameObject ability2BasePrefab;
    public float ability2CoolDown;
    //public float ability2ProjectileForce;
    //public float ability2KnockBackForce;
    public float ability2SlideVelocity;

    // Particles

    // Sound
}
