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
        private Health _health = new Health(100);
        
        [SerializeField]
        private float _maxSpeed = 10f;

        [SerializeField]
        private float _acceleration = 2f;

        [SerializeField]
        private float _amplitude = 1.5f;
        
        [SerializeField]
        private float _period = 0.5f;

        private VehicleSkinRenderer _skinRenderer;
        
        private float _movingTime = 0;
        private float _currentSpeed = 0;
        private bool _isMoving = false;

        public bool IsDead => _health.IsDead;

        [Inject]
        private void Construct()
        {
            _skinRenderer = GetComponentInChildren<VehicleSkinRenderer>(true);
        }
        
        public void StartMoving()
        {
            _movingTime = 0;
            _currentSpeed = 0;
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

            float stirring = Mathf.Sin(_period * _movingTime) * _amplitude;
            float newPosZ = position.z + _currentSpeed * Time.deltaTime;
            
            transform.rotation = Quaternion.Euler(0, stirring, 0);
            transform.position = new Vector3(stirring, position.y, newPosZ);
            
            _movingTime += Time.deltaTime;
            
            _currentSpeed += _acceleration * Time.deltaTime;
            _currentSpeed = Mathf.Min(_currentSpeed, _maxSpeed);
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            _skinRenderer.AnimateTakeDamage();
        }

        public void Die()
        {
            _isMoving = false;
            _skinRenderer.AnimateDeath();
        }
    }
}