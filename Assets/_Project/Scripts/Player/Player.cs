namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Vehicle))]
    public class Player : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        
        public Bounds Bounds => _vehicle.Bounds;

        private GameFlowController _gameFlowController;
        private Vehicle _vehicle;
        private Turret _turret;

        private bool _isAiming;

        [Inject]
        private void Construct(GameFlowController gameFlowController)
        {
            _gameFlowController = gameFlowController;
            _vehicle = GetComponent<Vehicle>();
            _turret = GetComponentInChildren<Turret>(true);
        }

        public void Activate()
        {
            _vehicle.StartMoving();
            _turret.StartShooting();
        }

        public void TakeDamage(int damage)
        {
            bool isAlreadyDead = _vehicle.IsDead;
            
            _vehicle.TakeDamage(damage);
            
            if (_vehicle.IsDead && !isAlreadyDead)
            {
                Die();
            }
        }

        private void Die()
        {
            _vehicle.Die();
            _turret.StopShooting();
            _gameFlowController.PlayerDeath();
        }

        public void SetAim(Vector3 direction)
        {
            _turret.SetAim(direction);
        }
    }
}