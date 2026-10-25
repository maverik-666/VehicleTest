namespace Project
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        private TrailRenderer _trailRenderer;

        [SerializeField]
        private ParticleSystem _hitVfx;
        
        [SerializeField]
        private float _speed = 20;

        [SerializeField]
        private int _maxSpeedPerStep = 10;
        
        [SerializeField]
        private int _attackDamage = 50;

        [SerializeField]
        private float _radius = 0.1f;

        [SerializeField]
        private float _autoReleaseDistance = 100;

        private Vector2 _startPosition2d;
        private EnemyRegistry _enemyRegistry;
        private Action<Projectile> _returnToPool;

        private bool _isDepleted = true;
            
        public void Init(EnemyRegistry enemyRegistry, Action<Projectile> returnToPool)
        {
            _enemyRegistry = enemyRegistry;
            _returnToPool = returnToPool;
            
            
            _trailRenderer.emitting = false;
            
        }

        public void Launch(Vector3 position, Vector3 direction)
        {
            _startPosition2d = new Vector2(position.x, position.z); 
            
            transform.position = position;
            transform.forward = direction;
            
            _trailRenderer.emitting = true;
            _renderer.enabled = true;
            _isDepleted = false;
        }
        
        private void Update()
        {
            if (_isDepleted)
            {
                return;
            }

            float steps = _speed / _maxSpeedPerStep;
            for (int i = 0; i < steps; i++)
            { 
                if (i + 1 > steps)
                { 
                    MoveForward(steps - Mathf.FloorToInt(steps) * _maxSpeedPerStep);
                }
                else
                { 
                    MoveForward(i + 1 * _maxSpeedPerStep);
                }

                if (_isDepleted)
                {
                    return;
                }
            }
        }

        private void MoveForward(float speed)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);

            var position2d = new Vector2(transform.position.x, transform.position.z);

            if (Vector2.Distance(position2d, _startPosition2d) > _autoReleaseDistance)
            {
                Release();
                return;
            }
            
            for (int i = _enemyRegistry.Count - 1; i >= 0; i--)
            {
                var enemy = _enemyRegistry.Items[i];
                var enemyPosition2d = new Vector2(enemy.Position.x, enemy.Position.z); 
                
                if (Vector2.Distance(position2d, enemyPosition2d) - enemy.Radius - _radius < 0)
                {
                    HitEnemy(enemy);
                    return;
                }
            }
        }

        private void HitEnemy(Enemy enemy)
        {
            enemy.TakeDamage(_attackDamage);
            //_hitVfx.Play();
            
            Release();
        }

        private void Release()
        {
            _trailRenderer.emitting = false;
            _renderer.enabled = false;

            _isDepleted = true;

            DOTween.Sequence().SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .AppendInterval(5)
                .AppendCallback(ReturnToPool);
        }

        private void ReturnToPool()
        {
            _returnToPool.Invoke(this);
        }
        
        public void Reset()
        {
            
        }
    }
}