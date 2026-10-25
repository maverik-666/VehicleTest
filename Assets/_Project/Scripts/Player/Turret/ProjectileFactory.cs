namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;
    using UnityEngine.Pool;
    using Random = UnityEngine.Random;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class ProjectileFactory : MonoBehaviour
    {
        [SerializeField]
        private Projectile _projectilePf;

        [SerializeField]
        private int _defaultCapacity = 5;
        
        [SerializeField]
        private int _maxSize = 20;

        private EnemyRegistry _enemyRegistry;
        private ObjectPool<Projectile> _pool;

        [Inject]
        private void Construct(EnemyRegistry enemyRegistry)
        {
            _enemyRegistry = enemyRegistry;
            _pool = new ObjectPool<Projectile>(
                PoolCreate, 
                PoolGet,
                PoolRelease,
                PoolDestroy,
                true,
                _defaultCapacity,
                _maxSize);
        }
        
        private Projectile PoolCreate()
        {
            var projectile = Instantiate(_projectilePf,  Vector3.zero, Quaternion.Euler(0, Random.Range(0, 360), 0), transform); 
            projectile.Init(_enemyRegistry, Release);
            
            projectile.gameObject.SetActive(false);
            
            return projectile;
        }
        
        private void PoolGet(Projectile projectile)
        {
            projectile.Reset();
        }

        private void PoolRelease(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        private void PoolDestroy(Projectile projectile)
        {
            Destroy(projectile.gameObject);
        }
        
        public Projectile Create()
        {
            var projectile = _pool.Get();
            return projectile;
        }

        private void Release(Projectile projectile)
        {
            _pool.Release(projectile);
        }

        private void OnDestroy()
        {
            _pool.Dispose();
        }
    }
}