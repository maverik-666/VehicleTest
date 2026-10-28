namespace Project
{
    using System;
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Vehicle))]
    public class Player : MonoBehaviour
    {
        private Vehicle _vehicle;
        private Turret _turret;
        
        private bool _isAiming;

        public event Action Died;
        
        public Vector3 Position => transform.position;
        
        public Bounds Bounds => _vehicle.Bounds;
        
        private void Awake()
        {
            _vehicle = GetComponent<Vehicle>();
            _turret = GetComponentInChildren<Turret>(true);
        }
        
        public void Activate()
        {
            _vehicle.StartMoving();
            _turret.StartShooting();
        }

        public void Deactivate()
        {
            _vehicle.StopMoving();
            _turret.StopShooting();
        }

        public void TakeDamage(int damage)
        {
            if (_vehicle.IsDead)
            {
                return;
            }
            
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
            Died?.Invoke();
        }

        public void SetAim(Vector3 direction)
        {
            _turret.SetAim(direction);
        }
        
        public void ResetState()
        {
            _vehicle.ResetState();
            _turret.ResetState();
        }
    }
}