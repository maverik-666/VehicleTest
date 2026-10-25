using Project;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[Enemy] Attacks Player", category: "Action", id: "19b7b6ed4956dae59b9a6ff2a233ab9f")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Enemy;

    protected override Status OnStart()
    { 
        Enemy.Value.AttackPlayer();
        return Status.Success;
    }
}

