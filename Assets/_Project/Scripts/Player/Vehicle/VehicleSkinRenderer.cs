namespace Project
{
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class VehicleSkinRenderer : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] _renderers;

        [SerializeField]
        private Color _takeDamageColor = Color.white;

        [SerializeField]
        private float _flashDuration = 0.15f;
        
        [SerializeField]
        private Color _deadColor = Color.gray;

        [SerializeField]
        private ParticleSystem _deathVfx;
        
        [SerializeField]
        private Transform[] _wheels;
        
        [SerializeField]
        private float _rotationSpeed = 30;
        
        [SerializeField]
        private float _wobbleStrength = 0.15f;
        
        [SerializeField]
        private float _wobbleDuration = 0.15f;
     
        [SerializeField]
        private float _wobbleVibrato = 0.15f;
        
        private Dictionary<Renderer, MaterialFlasher> _materialFlashers;

        private Tween _shakeTween; 
        
        public void Init()
        {
            _materialFlashers = new Dictionary<Renderer, MaterialFlasher>();
            foreach (var renderer in _renderers)
            {
                _materialFlashers.Add(renderer, new MaterialFlasher(renderer.gameObject, renderer));
            }
        }
        
        public void AnimateMovement()
        {
            foreach (var wheel in _wheels)
            {
                wheel.Rotate(Vector3.right, _rotationSpeed * Time.deltaTime);
            }
        }

        [NaughtyAttributes.Button]
        public void AnimateTakeDamage()
        {
            foreach (var renderer in _renderers)
            {
                _materialFlashers[renderer].FlashColor(_takeDamageColor, _flashDuration);
            }
            
            _shakeTween?.Kill(true); 
            _shakeTween = transform.DOPunchScale(Vector3.one * _wobbleDuration, _wobbleVibrato, elasticity: 0.5f)
                .SetLink(transform.gameObject, LinkBehaviour.KillOnDestroy);
        }

        public void AnimateDeath()
        {
            foreach (var renderer in _renderers)
            {
                _materialFlashers[renderer].SetColor(_deadColor);
            }
            
            _deathVfx.Play();
        }

        public void Reset()
        {
            foreach (var renderer in _renderers)
            {
                _materialFlashers[renderer].Clear();
            }
        }
    }
}