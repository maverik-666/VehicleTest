namespace Project
{
    using NaughtyAttributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class Vehicle : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 10f;

        [SerializeField]
        private float _amplitude = 1.5f;
        
        [SerializeField]
        private float _period = 0.5f;

        private float _movingTime = 0;
        private bool _isMoving = false;
        
        [Button("Start Move")]
        private void StartMove()
        {
            _movingTime = 0;
            _isMoving = true;
        }
        
        public void Update()
        {
            if (_isMoving)
            {
                MoveForward();
            }
        }

        private void MoveForward()
        {
            Vector3 position = transform.position;

            float stirring = Mathf.Sin(_period * _movingTime) * _amplitude;
            float newPosZ = position.z + _speed * Time.deltaTime;
            
            transform.rotation = Quaternion.Euler(0, stirring, 0);
            transform.position = new Vector3(stirring, position.y, newPosZ);
            _movingTime += Time.deltaTime;
        }
    }
}