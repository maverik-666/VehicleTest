namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Vehicle))]
    public class Player : MonoBehaviour
    {
        public Vector3 Position => transform.position;

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
            _vehicle.TakeDamage(damage);

            if (_vehicle.IsDead)
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