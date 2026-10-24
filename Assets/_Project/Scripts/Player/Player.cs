namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Vehicle))]
    public class Player : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        [Inject]
        private void Construct()
        {
            
        }
    }
}