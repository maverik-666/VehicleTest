using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Vector To Position", story: "Set [Vector] To [Target] position", category: "Action", id: "c17e6472f68bf54464d3165370d1a6aa")]
public partial class SetVectorToPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> Vector;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    protected override Status OnStart()
    {
        Vector.Value = Target.Value.position;
        
        return Status.Success;
    }
}

