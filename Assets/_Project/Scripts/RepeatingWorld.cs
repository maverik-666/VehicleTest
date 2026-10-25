namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class RepeatingWorld : MonoBehaviour
    {
        [SerializeField]
        private float _chunkSize = 75f;
        
        [SerializeField]
        private Transform _chunk;

        private Player _player;
        
        private float _startPos;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
            _startPos = _player.Position.z;
        }
        
        private void LateUpdate()
        {
            float delta = _player.Position.z - _startPos;
            float positionZ = 75 * (Mathf.RoundToInt(delta / 75));
            transform.position = new Vector3(0, 0, positionZ);
        } 
    }
}