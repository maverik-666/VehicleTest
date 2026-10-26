namespace Project
{
    using System;
    using Reflex.Attributes;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class InputSystem : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _pointerActionReference;

        [SerializeField]
        private InputActionReference _tapActionReference;
        
        private InputAction _pointerAction;
        private InputAction _tapAction;
        
        private Vector2 _pointerScreenPosition;

        public Vector2 PointerScreenPosition => _pointerScreenPosition;

        public bool IsPressed => _tapAction.IsPressed();

        public event Action Click;

        [Inject]
        private void Construct()
        { 
            _pointerAction = _pointerActionReference.action;
            _tapAction = _tapActionReference.action;
            
            _pointerAction.Enable();
            _tapAction.Enable();
        }
        
        private void Update()
        {
            _pointerScreenPosition = _pointerAction.ReadValue<Vector2>();

            if (_tapAction.WasPressedThisFrame())
            {
                Click?.Invoke();
            }
        }

        private void OnDestroy()
        {
            _pointerAction.Disable();
            _tapAction.Disable();
        }
    }
}