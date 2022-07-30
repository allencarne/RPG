using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    public new string name;
    public float health;
    public float maxHealth;
    public float speed;

    public WeaponScriptableObject weapon;
}
