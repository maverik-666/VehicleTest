namespace Project
{
    using System;
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Vehicle))]
    public class Player : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        
        public Bounds Bounds => _vehicle.Bounds;
        
        private Vehicle _vehicle;
        private Turret _turret;
        
        private Action _deathCallback;
        private bool _isAiming;

        public void Init(Action deathCallback)
        {
            _deathCallback = deathCallback;
            
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
            _deathCallback.Invoke();
        }

        public void SetAim(Vector3 direction)
        {
            _turret.SetAim(direction);
        }
        
        public void Reset()
        {
            _vehicle.Reset();
            _turret.Reset();
        }
    }
}