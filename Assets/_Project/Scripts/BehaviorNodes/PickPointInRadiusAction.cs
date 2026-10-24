using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Pick Point On Radius", story: "Pick [Location] Around [WorldPosition]", category: "Action", id: "6167488197f57526de3e82440bef66bd")]
public partial class PickPointInRadiusAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> Location;
    [SerializeReference] public BlackboardVariable<Vector3> WorldPosition;
    [SerializeReference] public BlackboardVariable<float> Radius = new BlackboardVariable<float>(1);

    protected override Status OnStart()
    {
        var randomOnCircle = UnityEngine.Random.insideUnitCircle.normalized  * Radius.Value;
        var randomPoint = new Vector3(randomOnCircle.x, 0, randomOnCircle.y);
        
        Location.Value = WorldPosition.Value + randomPoint;
        return Status.Success;
    }
}

