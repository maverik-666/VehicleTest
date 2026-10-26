namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class EnemyCollisionResolver : MonoBehaviour
    {
        [SerializeField]
        private int _collisionDamage = 15;

        private Player _player;
        private EnemyRegistry _enemyRegistry;
        
        [Inject]
        private void Construct(Player player, EnemyRegistry enemyRegistry)
        {
            _player = player;
            _enemyRegistry = enemyRegistry;
        }

        private void FixedUpdate()
        {
            for (int i = _enemyRegistry.Items.Count - 1; i >= 0; i--)
            {
                var enemy = _enemyRegistry.Items[i];
                if (_player.Bounds.Contains(enemy.Position))
                {
                    _player.TakeDamage(_collisionDamage);
                    enemy.Die();
                }
            }
        }
    }
}