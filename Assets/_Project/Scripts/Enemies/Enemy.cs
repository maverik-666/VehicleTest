namespace Project
{
    using Reflex.Attributes;
    using Unity.Behavior;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(BehaviorGraphAgent))]
    public class Enemy : MonoBehaviour
    {
        private Player _player;
        private BehaviorGraphAgent _agent;

        public Vector3 Position => transform.position;
        
        [Inject]
        private void Construct(Player player)
        {
            _agent = GetComponent<BehaviorGraphAgent>();
            _agent.Init();
            
            _agent.BlackboardReference.SetVariableValue("Player", player.gameObject);
        }

        public void StartPatrol()
        { 
            _agent.BlackboardReference.SetVariableValue("CanPatrol", true);
        }
        
        public void Reset()
        {
            _agent.BlackboardReference.SetVariableValue("CanPatrol", false);
            _agent.Restart();
        }
    }
}