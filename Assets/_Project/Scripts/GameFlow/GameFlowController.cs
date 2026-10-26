namespace Project
{
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class GameFlowController : MonoBehaviour
    {
        private GameState _gameState;

        [Inject]
        private Cover _cover;
        
        [Inject]
        private IntroScreen _introScreen;
        
        [Inject]
        private LoseScreen _loseScreen;

        [Inject]
        private WinScreen _winScreen;

        private WinConditionObserver _winConditionObserver;
        
        private InputSystem _inputSystem;
        
        private EnemySpawner _enemySpawner;
        private Player _player;
        
        [Inject]
        private void Construct( 
            WinConditionObserver winConditionObserver,
            InputSystem inputSystem,
            EnemySpawner enemySpawner,
            Player player)
        {
            _gameState = GameState.Intro;
            
            _winConditionObserver = winConditionObserver;
            _winConditionObserver.Init(PlayerCloseToDestination, PlayerReachedDestination);

            _player = player;
            _player.Init(PlayerDied);
            
            _inputSystem = inputSystem;
            _enemySpawner = enemySpawner;

            _inputSystem.Click += OnClick;
        }
        
        private void OnDisable()
        {
            _inputSystem.Click -= OnClick;
        }

        private void OnClick()
        {
            switch (_gameState)
            {
                case GameState.Intro:
                { 
                    StartGame();
                    break;
                }
                case GameState.Gameplay:
                {
                    break;
                }
                case GameState.Lose:
                {
                    Reload();
                    break;
                }
                case GameState.Win:
                {
                    Reload();
                    break;
                }
            }
        }
        
        private void StartGame()
        {
            _gameState = GameState.Gameplay;

            _introScreen.Hide();
                    
            _enemySpawner.EnablePatrol();
            _enemySpawner.EnableSpawn();
            
            _player.Activate();
            _winConditionObserver.StartObserving();
        }
        
        private void PlayerDied()
        {
            if(_gameState == GameState.Gameplay)
                Lose();
        }
        
        private void Lose()
        {
            _gameState = GameState.Lose;
            _enemySpawner.DisablePatrol();
            
            _winConditionObserver.StopObserving();
            _loseScreen.Show();
        }

        private void PlayerCloseToDestination()
        {
            _enemySpawner.DisableSpawn();
        }

        private void PlayerReachedDestination()
        {
            if(_gameState == GameState.Gameplay)
                Win();
        }

        private void Win()
        {
            _gameState = GameState.Win;
            
            _enemySpawner.DisablePatrol();
            _enemySpawner.DisableSpawn();
            
            _player.Deactivate();
            
            _winConditionObserver.StopObserving();
            _winScreen.Show();
        } 

        private void Reload()
        {
            _cover.FadeInOut(ResetGame);
        }
        
        private void ResetGame()
        {
            _player.Reset();
            _enemySpawner.Reset();
            _enemySpawner.EnableSpawn();
            
            _loseScreen.HideInstant();
            _winScreen.HideInstant();
            
            _introScreen.ShowInstant();
            _gameState = GameState.Intro;
        }
    }
}