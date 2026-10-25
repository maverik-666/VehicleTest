namespace Project
{
    using System.Collections.Generic;
    using NaughtyAttributes;
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class EnemySpawner : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField]
        private float _spawnRate = 0.5f;

        [SerializeField]
        private float _amplitude = 2f;

        [SerializeField]
        private float _spread = 1f;

        [SerializeField]
        private float _step = 2f;
        
        [SerializeField]
        private float _disposeDistance = 20;

        [MinMaxSlider(0, maxValue:100)]
        [SerializeField]
        private Vector2 _spawnRadiusRange;
        
        private Player _player;
        private EnemyFactory _enemyFactory;
        
        private List<Enemy> _enemies = new List<Enemy>();

        private float _lastPlayerPositionZ;
        
        [Inject]
        private void Construct(Player player, EnemyFactory enemyFactory)
        {
            _player = player;
            _enemyFactory = enemyFactory;
        }

        private void Start()
        {
            float playerPositionZ = _player.Position.z;
            TrySpawnOnRange(playerPositionZ + _spawnRadiusRange.x, playerPositionZ + _spawnRadiusRange.y);
        }

        private void Update()
        {
            DisposeBehind();
            TrySpawnAhead();
        }
        
        private void DisposeBehind()
        {
            float playerPositionZ = _player.Position.z;
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemies[i];
                if (playerPositionZ - enemy.Position.z > _disposeDistance)
                {
                    _enemyFactory.Dispose(enemy);
                    _enemies.RemoveAt(i);
                }
            }
        }
        
        private void TrySpawnAhead()
        {
            float playerPositionZ = _player.Position.z;
            float delta = playerPositionZ - _lastPlayerPositionZ;

            if (delta > _step)
            {
                TrySpawnOnRange(_lastPlayerPositionZ + _spawnRadiusRange.y, playerPositionZ + _spawnRadiusRange.y);
                _lastPlayerPositionZ = playerPositionZ; 
            }
        }

        private void TrySpawnOnRange(float start, float end)
        {
            int startSnapped = Mathf.CeilToInt(start / _step);
            int endSnapped = Mathf.FloorToInt(end / _step);
            
            for (int i = startSnapped; i <= endSnapped; i++)
            {
                float worldPosZ = i * _step;
                if (Random.Range(0,1f) > 1f - _spawnRate)
                {
                    SpawnOne(worldPosZ);
                }
            }
        }

        private void SpawnOne(float positionZ)
        {
            var position = new Vector3(Random.Range(-_spread, _spread), 0, positionZ);
            var enemy = _enemyFactory.Create(position);
            _enemies.Add(enemy);
        }
        
        
    }
}