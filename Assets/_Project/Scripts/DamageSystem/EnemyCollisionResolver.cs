namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class EnemyCollisionResolver : MonoBehaviour
    {
        private Player _player;
        private EnemyRegistry _enemyRegistry;
        
        [Inject]
        private void Construct(Player player, EnemyRegistry enemyRegistry)
        {
            _player = player;
            _enemyRegistry = enemyRegistry;
        }

        private void Update()
        {
            var playerBounds = _player.Bounds;
            for (int i = _enemyRegistry.Items.Count - 1; i >= 0; i--)
            {
                var enemy = _enemyRegistry.Items[i];
                var bounds = new Bounds(playerBounds.center, playerBounds.size);
                bounds.Expand(enemy.Radius * 2f);
                
                if (bounds.Contains(enemy.Position))
                {
                    _player.TakeDamage(enemy.AttackDamage);
                    enemy.Die();
                }
            }
        }
        
        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}