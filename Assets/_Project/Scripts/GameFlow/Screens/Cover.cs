namespace Project
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class Cover : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private Canvas _canvas;
        
        private void Awake()
        {
            _canvas = GetComponentInChildren<Canvas>(true);
            _canvas.gameObject.SetActive(true);
            
            _canvasGroup.alpha = 0;
        }

        public void FadeInOut(Action middleCallback)
        {
            _canvasGroup.alpha = 0;
            DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1, 0.3f))
                .AppendCallback(() => middleCallback?.Invoke())
                .Append(_canvasGroup.DOFade(0, 0.3f));
        }
    }
}