using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    public new string name;
    public int health;
    public int maxHealth;
    public float speed;

    public WeaponScriptableObject weapon;
}
