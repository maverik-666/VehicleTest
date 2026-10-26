namespace Project
{
    using System;
    using DG.Tweening;
    using Reflex.Attributes;
    using Unity.Behavior;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(BehaviorGraphAgent))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private Health _health = new Health(100);

        [SerializeField]
        private int _attackDamage = 15;

        [SerializeField]
        private float _radius = 0.5f;

        [SerializeField]
        private HealthBar _healthBar;
        
        private Action<Enemy> _returnToPool;
        
        private Player _player;
        private BehaviorGraphAgent _agent;
        private EnemySkinRenderer _skinRenderer;

        public Vector3 Position => transform.position;

        public float Radius => _radius;

        public bool IsDead => _health.IsDead;
        
        public void Init(Player player, Action<Enemy> returnToPool)
        {
            _healthBar.Init(_health);
            _healthBar.HideInstant();
            
            _player = player;
            _returnToPool = returnToPool;
            
            _agent = GetComponent<BehaviorGraphAgent>();
            _agent.Init();

            _agent.BlackboardReference.SetVariableValue("Player", _player.gameObject);
            
            _skinRenderer = GetComponentInChildren<EnemySkinRenderer>(true);
            _skinRenderer.Init(OnHitLanded);
        }

        public void StartPatrol()
        { 
            _agent.BlackboardReference.SetVariableValue("CanAggro", true);
        }
        
        public void DisablePatrol()
        { 
            _agent.BlackboardReference.SetVariableValue("CanAggro", false);
        }
        
        public void AttackPlayer()
        {
            _skinRenderer.AnimateAttack();
        }

        private void OnHitLanded()
        {
            _player.TakeDamage(_attackDamage);
            Die();
        }
        
        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            
            _healthBar.Show();
            _healthBar.UpdateHealth();
            
            _skinRenderer.AnimateTakeDamage();

            if (_health.IsDead)
            {
                Die();
            }
        }
        
        public void Reset()
        {
            _health.SetCurrentHealth(_health.Max);
            _healthBar.HideInstant();
            _healthBar.UpdateHealthInstant();
            
            _agent.BlackboardReference.SetVariableValue("CanPatrol", false);
            _agent.Restart();
            _agent.enabled = true;
            
            _skinRenderer.Reset();
        }

        public void Die()
        {
            _skinRenderer.AnimateDeath();
            _healthBar.Hide();
            _agent.End();
            _agent.enabled = false;

            Dispose();
        }

        public void DieSilent()
        {
            _agent.End();
            _agent.enabled = false;
            
            Dispose();
        }
        
        private void Dispose()
        {
            _returnToPool.Invoke(this);
        }
    }
}