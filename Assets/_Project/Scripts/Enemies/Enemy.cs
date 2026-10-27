namespace Project
{
    using System;
    using Reflex.Attributes;
    using Unity.Behavior;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(BehaviorGraphAgent))]
    public class Enemy : MonoBehaviour
    {
        private static readonly string CanAggroId = "CanAggro";
        
        [SerializeField]
        private Health _health = new Health(100);

        [SerializeField]
        private HealthBar _healthBar;
        
        [SerializeField]
        private int _attackDamage = 15;

        [SerializeField]
        private float _wanderingSpeed = 0.75f;

        [SerializeField]
        private float _chasingSpeed = 5;

        [SerializeField]
        private float _visionDistance = 20;
        
        [SerializeField]
        private float _radius = 0.5f;

     
        private Player _player;
        private BehaviorGraphAgent _agent;
        private EnemySkinRenderer _skinRenderer;

        public event Action<Enemy> ReturnToPool; 
        
        public Vector3 Position => transform.position;

        public float Radius => _radius;

        public bool IsDead => _health.IsDead;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        private void Awake()
        {
            _healthBar.Init(_health);
            _healthBar.HideInstant();
            
            _agent = GetComponent<BehaviorGraphAgent>();
            
            _skinRenderer = GetComponentInChildren<EnemySkinRenderer>(true);
            _skinRenderer.Init(OnHitLanded);
        }

        private void Start()
        {
            _agent.BlackboardReference.SetVariableValue("Player", _player.gameObject);
        }

        public void StartPatrol()
        { 
            _agent.BlackboardReference.SetVariableValue(CanAggroId, true);
        }
        
        public void DisablePatrol()
        { 
            _agent.BlackboardReference.SetVariableValue(CanAggroId, false);
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
        
        public void ResetState()
        {
            _health.SetCurrentHealth(_health.Max);
            _healthBar.HideInstant();
            _healthBar.UpdateHealthInstant();
            
            _agent.BlackboardReference.SetVariableValue(CanAggroId, false);
            _agent.Restart();
            _agent.enabled = true;
            
            _skinRenderer.ResetState();
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
            ReturnToPool?.Invoke(this);
        }
    }
}