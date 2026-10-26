namespace Project
{
    using DG.Tweening;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class IntroScreen : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private RectTransform _text;

        [SerializeField]
        private float _pulseScale = 1.2f;
        
        [SerializeField]
        private float _pulseDuration = 2;

        private Canvas _canvas;
        private Tween _fadeTween;
        
        private void Awake()
        {
            _canvas = GetComponentInChildren<Canvas>(true);
            _canvas.gameObject.SetActive(true);
            
            _text.transform.DOScale(_pulseScale, _pulseDuration)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
        
        public void ShowInstant()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.blocksRaycasts = false;
            
            _fadeTween?.Kill();
            _fadeTween = _canvasGroup.DOFade(0, 0.3f);
        }
    }
}