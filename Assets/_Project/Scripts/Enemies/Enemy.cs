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
        
        [Inject]
        private void Construct(Player player)
        {
            _agent = GetComponent<BehaviorGraphAgent>();
            _agent.Init();
            
            _agent.BlackboardReference.SetVariableValue("Player", player.gameObject);
        }

        public void Reset()
        {
            
        }
    }
}