using System;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] ScriptableObject attackBehaviourReference;
    public IAttackBehaviour AttackBehaviour { get; private set; }

    [SerializeField] ScriptableObject dashBehaviourReference;
    public IDashBehaviour DashBehaviour { get; private set; }

    private void Awake()
    {
        AttackBehaviour = (IAttackBehaviour)attackBehaviourReference;
        DashBehaviour = (IDashBehaviour)dashBehaviourReference;
    }
}
