namespace Project
{
    using DG.Tweening;
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class Turret : MonoBehaviour
    {
        [SerializeField]
        private Transform _rotatingTransform;
        
        [SerializeField]
        private Transform _recoilingTransform;

        [SerializeField]
        private float _rotationSpeed = 30;

        [SerializeField]
        private float _maxRotationAngle = 45;
        
        [SerializeField]
        private TurretRay _turretRay;
        
        [SerializeField]
        private Transform _projectileSpawnPoint;

        [SerializeField]
        private ParticleSystem _shootVfx;
        
        [SerializeField]
        private float _shootInterval = 3;

        [SerializeField]
        private float _recoilDistance = 0.3f;

        [SerializeField]
        private float _recoilDuration = 0.5f;
        
        private ProjectileFactory _projectileFactory;

        private bool _isShooting = false;
        private Timer _shootTimer;

        private Tween _recoilTween;

        private float _targetRotation = 0;
        private float _currentRotation = 0;
        
        [Inject]
        private void Construct(ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
            _shootTimer = new Timer();
        }
        
        public void StartShooting()
        {
            _isShooting = true;
            _shootTimer.Start(_shootInterval);
            _turretRay.Show();
        }

        public void StopShooting()
        {
            _isShooting = false;
            _turretRay.Hide();
        }

        public void SetAim(Vector3 direction)
        {
            _targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }

        private void Update()
        {
            if (!_isShooting)
            {
                return;
            }
            
            _currentRotation = Mathf.LerpAngle(_currentRotation, _targetRotation, Time.deltaTime * _rotationSpeed);
            _currentRotation = Mathf.Clamp(_currentRotation, -_maxRotationAngle, _maxRotationAngle);
            _rotatingTransform.rotation = Quaternion.Euler(0, _currentRotation, 0);
            
            _shootTimer.Tick(Time.deltaTime);

            if (_shootTimer.IsStopped)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            var projectile = _projectileFactory.Create();
            projectile.Launch(_projectileSpawnPoint.position, _projectileSpawnPoint.forward);
            projectile.gameObject.SetActive(true);
            
            _shootVfx.Play();
            
            _shootTimer.Start(_shootInterval);
            
            _recoilTween?.Kill(true);
            _recoilTween = _recoilingTransform.DOLocalMove(-transform.forward * _recoilDistance, _recoilDuration/2f)
                .SetEase(Ease.OutCubic)
                .SetRelative()
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}