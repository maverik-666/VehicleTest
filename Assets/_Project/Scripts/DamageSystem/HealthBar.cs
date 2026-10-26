namespace Project
{
    using DG.Tweening;
    using Reflex.Attributes;
    using Unity.Mathematics.Geometry;
    using UnityEngine;
    using UnityEngine.UI;
    using Canvas = Unity.AppUI.UI.Canvas;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Slider _fillSlider;

        [SerializeField]
        private Slider _lateFillSlider;

        [SerializeField]
        private float _fillSpeed = 3;

        [SerializeField]
        private float _lateFillSpeed = 3;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private float _targetValue;
        private float _currentFillValue;
        private float _currentLateFillValue;

        private Camera _camera;
        
        private Health _health;
        private Tween _fadeTween;

        private bool _isShown;

        [Inject]
        private void Construct(Camera camera)
        {
            _camera = camera;
        }
        
        public void Init(Health health)
        {
            _health = health;
        }

        public void UpdateHealth()
        {
            _targetValue = (float) _health.Current / _health.Max;
        }

        public void UpdateHealthInstant()
        {
            _targetValue = (float) _health.Current / _health.Max;
            _currentFillValue = _targetValue;
            _currentLateFillValue = _targetValue;
        }

        public void Show()
        {
            if (_isShown)
            {
                return;
            }
            
            _isShown = true;
            _fadeTween?.Kill();
            _fadeTween = _canvasGroup.DOFade(1, 0.3f)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
        
        public void Hide()
        {
            if (!_isShown)
            {
                return;
            }
            
            _isShown = false;
            _fadeTween?.Kill();
            _fadeTween = _canvasGroup.DOFade(0, 0.3f)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public void HideInstant()
        {
            _isShown = false;
            _canvasGroup.alpha = 0;
        }

        private void Update()
        { 
            transform.rotation = _camera.transform.rotation;
            
            _currentFillValue = Mathf.MoveTowards(_currentFillValue, _targetValue, Time.deltaTime * _fillSpeed);
            _currentLateFillValue = Mathf.MoveTowards(_currentLateFillValue, _targetValue, Time.deltaTime * _lateFillSpeed);
            
            _fillSlider.value = _currentFillValue;
            _lateFillSlider.value = _currentLateFillValue;
        }
    }
}