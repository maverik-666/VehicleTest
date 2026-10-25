namespace Project
{
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class VehicleSkinRenderer : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _wheels;

        [SerializeField]
        private float _rotationSpeed = 30;

        
        
        public void AnimateMovement()
        {
            foreach (var wheel in _wheels)
            {
                wheel.Rotate(Vector3.right, _rotationSpeed * Time.deltaTime);
            }
        }

        public void AnimateTakeDamage()
        {
            
        }

        public void AnimateDeath()
        {
            
        }
    }
}