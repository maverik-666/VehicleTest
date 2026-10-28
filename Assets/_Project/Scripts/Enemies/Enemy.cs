namespace Project
{
    using System;
    using Reflex.Attributes;
    using UnityEngine;
    using UnityEngine.AI;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private HealthBar _healthBar;

        private EnemyConfig _config;
        private EnemyBrain _brain;
        private Health _health;
        
        private NavMeshAgent _navMeshAgent;
        private EnemySkinRenderer _skinRenderer;
        
        public event Action<Enemy> ReturnToPool; 
        
        public Vector3 Position => transform.position;

        public float Radius => _config.Radius;

        public int AttackDamage => _config.AttackDamage;

        [Inject]
        private void Construct(EnemyConfig config, Player player)
        {
            _config = config;
            _health = new Health(config.Health);
            
            _healthBar.Init(_health);
            
            _navMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
            
            _skinRenderer = GetComponentInChildren<EnemySkinRenderer>(true);
            _skinRenderer.Init(OnAttackEnded);
            
            _brain = new EnemyBrain(this, _config, player);
        }
        
        private void Update()
        {
            _brain.Tick(Time.deltaTime);
        }
        
        public void Idle()
        {
            _skinRenderer.AnimateIdle();
        }

        public void WalkTo(Vector3 position)
        {
            _navMeshAgent.speed = _config.WalkingSpeed;
            _navMeshAgent.SetDestination(position);
            _skinRenderer.AnimateWalk();
        }

        public void RunTo(Vector3 position)
        {
            _navMeshAgent.speed = _config.RunningSpeed;
            _navMeshAgent.SetDestination(position);
            _skinRenderer.AnimateRun();
        }
        
        public void Attack()
        {
            _skinRenderer.AnimateAttack();
        }

        private void OnAttackEnded()
        {
            _brain.OnAttackEnded();
        }
        
        public void EnableAggro()
        { 
            _brain.EnableAggro();
        }
        
        public void DisableAggro()
        { 
            _brain.DisableAggro();
        }
        
        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            
            _healthBar.Show();
            _healthBar.UpdateHealth();
            
            _skinRenderer.AnimateTakeDamage();

            _brain.OnTakeDamage();
            
            if (_health.IsDead)
            {
                Die();
            }
        }
        
        public bool HasReachedDestination()
        {
            return !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 0.1f;
        }

        public void Spawn(Vector3 position)
        {
            gameObject.SetActive(true);
            
            if (!_navMeshAgent.Warp(position))
            {
                Debug.LogWarning($"Enemy spawned off NavMesh at {position}", this);
            }

            _navMeshAgent.isStopped = false;
            
            _brain.Spawn(position);
        }

        public void ResetState()
        {
            _health.SetCurrentHealth(_health.Max);
            _healthBar.HideInstant();
            _healthBar.UpdateHealthInstant();

            _skinRenderer.ResetState();
        }

        public void Die()
        {
            _brain.Stop();
            _navMeshAgent.ResetPath();
            _navMeshAgent.isStopped = true;
            
            _skinRenderer.AnimateDeath();
            _healthBar.Hide();

            Dispose();
        }

        public void DieSilent()
        {
            _brain.Stop();
            _navMeshAgent.ResetPath();
            _navMeshAgent.isStopped = true;
            
            Dispose();
        }
        
        private void Dispose()
        {
            ReturnToPool?.Invoke(this);
        }
    }
}