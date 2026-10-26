namespace Project
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class WinScreen : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private RectTransform _title;
        
        [SerializeField]
        private RectTransform _tapText;

        [SerializeField]
        private float _pulseScale = 1.2f;
        
        [SerializeField]
        private float _pulseDuration = 2;

        private Canvas _canvas;
        private Tween _showTween;
        
        private void Awake()
        {
            _canvas = GetComponentInChildren<Canvas>(true);
            _canvas.gameObject.SetActive(true);
            
            _canvasGroup.alpha = 0;
        }

        public void Show(Action callback = null)
        {
            _title.transform.localScale = Vector3.zero;
            _tapText.transform.localScale = Vector3.zero;
            
            _canvasGroup.DOFade(1, 0.3f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            _showTween?.Kill();
            _showTween = DOTween.Sequence()
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .Append(_title.DOScale(1, 1).SetEase(Ease.OutBack))
                .Insert(0.3f, _tapText.DOScale(1, 1).SetEase(Ease.OutBack))
                .AppendCallback(() => callback?.Invoke())
                .AppendCallback(StartPulse);
        }

        private void StartPulse()
        {
            _showTween = _tapText.transform.DOScale(_pulseScale, _pulseDuration)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        } 
        
        public void HideInstant()
        {
            _showTween?.Kill();
            _canvasGroup.alpha = 0;
        }
    }
}