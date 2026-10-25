namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class PlayerAimer : MonoBehaviour
    {
        private Player _player;
        private InputSystem _inputSystem;
        private Camera _camera;
        
        [Inject]
        private void Construct(Player player, InputSystem inputSystem, Camera camera)
        {
            _player = player;
            _inputSystem = inputSystem;
            _camera = camera;
        }

        private void Update()
        {
            if (!_inputSystem.IsPressed)
            {
                return;
            }
            
            var ray = _camera.ScreenPointToRay(_inputSystem.PointerScreenPosition);

            float t = (-ray.origin.y) / ray.direction.y;
            var aimPoint = ray.origin + ray.direction * t;
            var direction = aimPoint - _player.Position;
            direction.y = 0;
            
            _player.SetAim(direction);
        }
    }
}