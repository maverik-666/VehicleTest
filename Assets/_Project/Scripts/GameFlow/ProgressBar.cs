namespace Project
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _barTransform;

        [SerializeField]
        private float _hiddenOffset = 1000;
        
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private float _speed = 3;

        private float _targetValue;
        private float _currentValue;

        private Tween _slideTween;

        private bool _isShown;

        public void UpdateProgress(float progress)
        {
            _targetValue = progress;
        }

        public void UpdateProgressInstant(float progress)
        {
            _targetValue = progress;
            _currentValue = progress;
        }

        public void Show()
        {
            if (_isShown)
            {
                return;
            }
            
            _isShown = true;
            _slideTween?.Kill(true);
            _slideTween = _barTransform.DOAnchorPosX(-_hiddenOffset, 1f)
                .SetRelative()
                .SetEase(Ease.OutBack)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
        
        public void Hide()
        {
            if (!_isShown)
            {
                return;
            }
            
            _isShown = false;
            _slideTween?.Kill(true);
            _slideTween = _barTransform.DOAnchorPosX(_hiddenOffset, 1f)
                .SetRelative()
                .SetEase(Ease.InBack)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public void HideInstant()
        {
            _isShown = false;
            
            _slideTween?.Kill();
            _barTransform.anchoredPosition = new Vector2(_hiddenOffset, 0);
        }

        private void Update()
        { 
            _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, Time.deltaTime * _speed);
            _slider.value = _currentValue;
        }
    }
}