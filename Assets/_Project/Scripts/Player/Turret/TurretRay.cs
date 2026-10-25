namespace Project
{
    using DG.Tweening;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class TurretRay : MonoBehaviour
    {
        [Range(0, 1f)]
        [SerializeField]
        private float _visibleAlpha;
        
        private SpriteRenderer _spriteRenderer;

        private Tween _tween;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.SetAlpha(0);
        }

        public void Show()
        {
            _tween?.Kill();
            _tween = _spriteRenderer.DOFade(_visibleAlpha, 0.5f)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public void Hide()
        {
            _tween?.Kill();
            _tween = _spriteRenderer.DOFade(0, 0.5f)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
    }
}