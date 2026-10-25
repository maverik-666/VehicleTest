namespace Project
{
    using NaughtyAttributes.Test;
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Vehicle))]
    public class Player : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        private GameFlowController _gameFlowController;
        private Vehicle _vehicle;
        
        [Inject]
        private void Construct(GameFlowController gameFlowController)
        {
            _gameFlowController = gameFlowController;
            _vehicle = GetComponent<Vehicle>();
        }

        public void StartMoving()
        {
            _vehicle.StartMoving();
        }

        public void TakeDamage(int damage)
        {
            _vehicle.TakeDamage(damage);

            if (_vehicle.IsDead)
            {
                _vehicle.Die();
                _gameFlowController.PlayerDeath();
            }
        }
    }
}