namespace Project
{
    using DG.Tweening;
    using UnityEngine;

    public class MaterialFlasher
    {
        private static readonly int _ColorId = Shader.PropertyToID("_Color");
        
        private readonly GameObject _gameObject;
        private readonly Renderer _renderer;
        private readonly MaterialPropertyBlock _propBlock;
        
        private Tween _flashTween;

        public MaterialFlasher(GameObject gameObject, Renderer renderer)
        {
            _gameObject = gameObject;
            _renderer = renderer;
            _propBlock = new MaterialPropertyBlock();
        }

        public void FlashColor(Color color, float duration)
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor(_ColorId, color);
            _renderer.SetPropertyBlock(_propBlock);

            _flashTween?.Kill();
            _flashTween = DOTween.Sequence()
                .SetLink(_gameObject, LinkBehaviour.KillOnDestroy)
                .AppendInterval(duration)
                .AppendCallback(Revert);
        }

        private void Revert()
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor(_ColorId, Color.clear);
            _renderer.SetPropertyBlock(_propBlock);
        }
    }
}