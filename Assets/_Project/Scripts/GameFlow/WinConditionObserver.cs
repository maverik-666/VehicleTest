namespace Project
{
    using System;
    using Reflex.Attributes;
    using UnityEngine;

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

        public event Action WinCallback;
        public event Action CloseWinCallback;

        private bool _isCloseWinReached = false;
        private bool _isObserving;
        
        [Inject]
        private void Construct(ProgressBar progressBar, Player player)
        {
            _progressBar = progressBar;
            _player = player;
        }

        private void Awake()
        {
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

        public void ResetState()
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
            CloseWinCallback?.Invoke();
        }
        
        private void Win()
        {
            WinCallback.Invoke();
        }
    }
}