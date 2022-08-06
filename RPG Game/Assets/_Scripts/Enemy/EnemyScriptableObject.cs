using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public new string name;
    public float health;
    public float maxHealth;
    public float speed;
    public float attackRange;
    public float aggroRange;

    public float attackCoolDown;
}