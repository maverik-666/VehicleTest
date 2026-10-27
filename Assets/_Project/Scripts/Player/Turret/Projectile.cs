namespace Project
{
    using System;
    using DG.Tweening;
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _vfx;

        [SerializeField]
        private ParticleSystem _hitVfx;
        
        [SerializeField]
        private float _speed = 20;

        [SerializeField]
        private int _attackDamage = 50;

        [SerializeField]
        private float _radius = 0.1f;

        [SerializeField]
        private float _autoReleaseDistance = 100;

        private Vector2 _startPositionXZ;
        private EnemyRegistry _enemyRegistry;

        private bool _isDepleted = true;

        public event Action<Projectile> ReturnToPool;
        
        [Inject]
        private void Construct(EnemyRegistry enemyRegistry)
        {
            _enemyRegistry = enemyRegistry;
        }

        public void Launch(Vector3 position, Vector3 direction)
        {
            _startPositionXZ = position.ToXZ(); 
            
            transform.position = position;
            transform.forward = direction;
            _vfx.gameObject.SetActive(true);
            _isDepleted = false;
        }
        
        private void Update()
        {
            if (_isDepleted)
            {
                return;
            }

            Vector3 start = transform.position;
            Vector3 end = start + transform.forward * (_speed * Time.deltaTime);
            
            if(TryHitEnemy(start, end, out var enemy, out var hitPoint))
            {
                transform.position = hitPoint;
                HitEnemy(enemy);
                return;
            }
            
            transform.position = end;
            
            if ((end.ToXZ() - _startPositionXZ).sqrMagnitude > _autoReleaseDistance * _autoReleaseDistance)
            {
                Release();
            }
        }

        private bool TryHitEnemy(Vector3 start, Vector3 end, out Enemy hitEnemy, out Vector3 hitPoint)
        {
            Vector2 a = start.ToXZ();
            Vector2 ab = end.ToXZ() - a;
            float abSqr = ab.sqrMagnitude;

            bool hitAny = false;
            hitEnemy = null;
            float closestT = 1;
            
            for (int i = 0; i < _enemyRegistry.Count; i++)
            {
                var enemy = _enemyRegistry.Items[i];
                Vector2 center = enemy.Position.ToXZ();
                
                float t = abSqr > 0 ? Mathf.Clamp01(Vector2.Dot(center - a, ab) / abSqr) : 0;
                Vector2 closestPoint = a + ab * t;
                
                float hitRadius = enemy.Radius + _radius;
                if ((center - closestPoint).sqrMagnitude < hitRadius * hitRadius && t <= closestT)
                {
                    closestT = t;
                    hitEnemy = enemy;
                    hitAny = true;
                }
            }
            
            hitPoint = Vector3.Lerp(start, end, closestT);
            
            return hitAny;
        }
        
        private void HitEnemy(Enemy enemy)
        {
            enemy.TakeDamage(_attackDamage);
            _hitVfx.Play();
            
            Release();
        }

        private void Release()
        {
            _vfx.gameObject.SetActive(false);

            _isDepleted = true;

            DOTween.Sequence().SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .AppendInterval(5)
                .AppendCallback(Dispose);
        }

        private void Dispose()
        {
            ReturnToPool?.Invoke(this);
        }
        
        public void ResetState()
        {
            _isDepleted = false;
        }
    }
}