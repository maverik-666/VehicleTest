namespace Project
{
    using Reflex.Attributes;
    using Reflex.Core;
    using Reflex.Injectors;
    using UnityEngine;
    using UnityEngine.Pool;
    using Random = UnityEngine.Random;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class ProjectileFactory : MonoBehaviour
    {
        [SerializeField]
        private Projectile _projectilePf;

        private Container _container;
        
        private ObjectPool<Projectile> _pool;

        [Inject]
        private void Construct(Container container, ProjectileConfig projectileConfig)
        {
            _container = container;
            
            _pool = new ObjectPool<Projectile>(
                PoolCreate, 
                PoolGet,
                PoolRelease,
                PoolDestroy,
                true,
                projectileConfig.PoolCapacity,
                projectileConfig.PoolMaxSize);
        }
        
        private Projectile PoolCreate()
        {
            var projectile = Instantiate(_projectilePf,  Vector3.zero, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
            GameObjectInjector.InjectRecursive(projectile.gameObject, _container);
            projectile.ReturnToPool += Release;
            projectile.gameObject.SetActive(false);
            
            return projectile;
        }
        
        private void PoolGet(Projectile projectile)
        {
            projectile.ResetState();
        }

        private void PoolRelease(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        private void PoolDestroy(Projectile projectile)
        {
            projectile.ReturnToPool -= Release;
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