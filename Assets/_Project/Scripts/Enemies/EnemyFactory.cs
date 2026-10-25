namespace Project
{
    using System;
    using Reflex.Attributes;
    using UnityEngine;
    using UnityEngine.Pool;
    using Random = UnityEngine.Random;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField]
        private Enemy _enemyPf;

        [SerializeField]
        private int _defaultCapacity = 50;
        
        [SerializeField]
        private int _maxSize = 200;

        private Player _player;
        private ObjectPool<Enemy> _pool;
        private EnemyRegistry _enemyRegistry;

        [Inject]
        private void Construct(EnemyRegistry enemyRegistry, Player player)
        {
            _enemyRegistry = enemyRegistry;
            _player = player;
            _pool = new ObjectPool<Enemy>(
                PoolCreate, 
                PoolGet,
                PoolRelease,
                PoolDestroy,
                true,
                _defaultCapacity,
                _maxSize);
        }
        
        private Enemy PoolCreate()
        {
            var enemy = Instantiate(_enemyPf,  Vector3.zero, Quaternion.Euler(0, Random.Range(0, 360), 0), transform); 
            enemy.Init(_player, Release);
            
            enemy.gameObject.SetActive(false);
            
            return enemy;
        }
        
        private void PoolGet(Enemy enemy)
        {
            enemy.Reset();
        }

        private void PoolRelease(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        private void PoolDestroy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
        
        public Enemy Create(Vector3 position)
        {
            var enemy = _pool.Get();
            enemy.transform.position = position;
            enemy.gameObject.SetActive(true);
            
            _enemyRegistry.Add(enemy);
            
            return enemy;
        }

        public void Release(Enemy enemy)
        {
            _enemyRegistry.Remove(enemy);
            _pool.Release(enemy);
        }

        private void OnDestroy()
        {
            _pool.Dispose();
        }
    }
}