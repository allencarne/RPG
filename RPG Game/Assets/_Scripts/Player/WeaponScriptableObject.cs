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

    [Header("Basic Attack 2 Ability")]
    //public float basicAttack2CoolDown;
    //public float basicAttack2SlideVelocity;
    public GameObject basicAttack2Prefab;
    //public GameObject basicAttack2HitEffect;
    //public float basicAttack2KnockBackForce;
    //public float basicAttack2ProjectileForce;

    [Header("Basic Attack 3 Ability")]
    //public float basicAttack3CoolDown;
    //public float basicAttack3SlideVelocity;
    public GameObject basicAttack3Prefab;
    //public GameObject basicAttack3HitEffect;
    //public float basicAttack3KnockBackForce;
    //public float basicAttack3ProjectileForce;

    [Header("Dash Ability")]
    public GameObject dashParticle;
    public float dashCoolDown;
    public float dashVelocity;
    public GameObject dashPrefab;
    public float dashProjectileForce;

    // Particles

    // Sound
}
