namespace Project
{
    using NaughtyAttributes;
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class Vehicle : MonoBehaviour
    {
        [SerializeField]
        private Collider _collider;
        
        [SerializeField]
        private Health _health = new Health(100);

        [SerializeField]
        private HealthBar _healthBar;
        
        [SerializeField]
        private float _maxSpeed = 10f;

        [SerializeField]
        private float _acceleration = 2f;

        [SerializeField]
        private float _stirringAmplitude = 1.5f;
        
        [SerializeField]
        private float _rotationAmplitude = 1.5f;
        
        [SerializeField]
        private float _amplitudeAcceleration = 0.1f;
        
        [SerializeField]
        private float _period = 0.5f;

        private VehicleSkinRenderer _skinRenderer;
        
        private float _movingTime = 0;
        private float _currentSpeed = 0;
        private float _currentAmplitude = 0;
        private bool _isMoving = false;

        public bool IsDead => _health.IsDead;

        public Bounds Bounds => _collider.bounds;
        
        [Inject]
        private void Construct()
        {
            _skinRenderer = GetComponentInChildren<VehicleSkinRenderer>(true);
            _skinRenderer.Init();
            
            _healthBar.Init(_health);
            _healthBar.HideInstant();
        }
        
        public void StartMoving()
        {
            _movingTime = 0;
            _currentSpeed = 0;
            _currentAmplitude = 0;
            
            _health.SetCurrentHealth(_health.Max);
            _healthBar.UpdateHealthInstant();
            
            _skinRenderer.StartMoving();
            
            _isMoving = true;
        }
        
        public void Update()
        {
            if (_isMoving)
            {
                MoveForward();
                _skinRenderer.AnimateMovement();
            }
        }

        private void MoveForward()
        {
            Vector3 position = transform.position;
            
            float stirring = Mathf.Cos(_period * _movingTime) * _rotationAmplitude * _currentAmplitude;
            float newPosX = Mathf.Sin(_period * _movingTime) * _stirringAmplitude * _currentAmplitude;
            float newPosZ = position.z + _currentSpeed * Time.deltaTime;
            
            transform.rotation = Quaternion.Euler(0, stirring, 0);
            transform.position = new Vector3(newPosX, position.y, newPosZ);
            
            _movingTime += Time.deltaTime;
            
            _currentSpeed += _acceleration * Time.deltaTime;
            _currentSpeed = Mathf.Min(_currentSpeed, _maxSpeed);
            
            _currentAmplitude += _amplitudeAcceleration * Time.deltaTime;
            _currentAmplitude = Mathf.Min(_currentAmplitude, 1);
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);

            if (!_health.IsDead)
            {
                _healthBar.Show(); 
                _healthBar.UpdateHealth();
            }
            
            _skinRenderer.AnimateTakeDamage();
        }

        public void Die()
        {
            _isMoving = false;
            
            _healthBar.Hide();
            
            _skinRenderer.AnimateDeath();
        }

        public void Reset()
        {
            _isMoving = false;
            _healthBar.HideInstant();
            
            _skinRenderer.Reset();
            
            transform.rotation = Quaternion.identity;
            transform.position = Vector3.zero;
            
        }
    }
}