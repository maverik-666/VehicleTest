namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class GameFlowController : MonoBehaviour
    {
        
        
        private EnemySpawner _enemySpawner;
        private Player _player;
        
        [Inject]
        private void Construct(EnemySpawner enemySpawner, Player player)
        {
            _enemySpawner = enemySpawner;
            _player = player;
        }

        [NaughtyAttributes.Button]
        public void StartGame()
        {
            _enemySpawner.EnablePatrol();
            _player.Activate();
        }
        
        public void PlayerDeath()
        {
            _enemySpawner.DisablePatrol();
        }
    }
}