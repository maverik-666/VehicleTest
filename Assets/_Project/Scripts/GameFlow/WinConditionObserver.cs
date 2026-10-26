namespace Project
{
    using System;
    using Reflex.Attributes;
    using UnityEngine;
    using UnityEngine.UIElements;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class WinConditionObserver : MonoBehaviour
    { 
        [SerializeField]
        private Transform _destination;

        [Range(0, 1)]
        [SerializeField]
        private float _closeWinStamp;
        
        private ProgressBar _progressBar;
        private Player _player;

        private Action _winCallback;
        private Action _closeWinCallback;

        private bool _isCloseWinReached = false;
        private bool _isObserving;
        
        [Inject]
        private void Construct(ProgressBar progressBar, Player player)
        {
            _progressBar = progressBar;
            _player = player;
        }

        public void Init(Action closeWinCallback, Action winCallback)
        {
            _winCallback = winCallback;
            _closeWinCallback = closeWinCallback;
            _progressBar.HideInstant();
        }

        public void StartObserving()
        {
            _isObserving = true;
            _progressBar.UpdateProgressInstant(0);
            _progressBar.Show();
        }

        public void StopObserving()
        {
            _isObserving = false;
            _progressBar.Hide();
        }

        public void Reset()
        {
            _isCloseWinReached = false;
            _progressBar.HideInstant();
            _progressBar.UpdateProgressInstant(0);
        }
        
        private void Update()
        {
            if (!_isObserving)
            {
                return;
            }
            
            float progress = _player.Position.z / _destination.position.z;
            _progressBar.UpdateProgress(progress);
            
            if (progress >= _closeWinStamp && !_isCloseWinReached)
            {
                CloseToWin();
            }
            
            if (progress >= 1)
            {
                Win();
            }
        }

        private void CloseToWin()
        {
            _isCloseWinReached = true;
            _closeWinCallback?.Invoke();
        }
        
        private void Win()
        {
            _winCallback.Invoke();
        }
    }
}