namespace Project
{
    using System;

    public class GameFlow : IDisposable
    {
        private GameState _gameState;

        private readonly Cover _cover;
        
        private readonly IntroScreen _introScreen;
        
        private readonly LoseScreen _loseScreen;

        private readonly WinScreen _winScreen;

        private readonly WinConditionObserver _winConditionObserver;
        
        private readonly InputHandler _inputHandler;
        
        private readonly EnemySpawner _enemySpawner;
        private readonly Player _player;
        
        public GameFlow( 
            Cover cover,
            IntroScreen introScreen,
            LoseScreen loseScreen,
            WinScreen winScreen,
            InputHandler inputHandler,
            WinConditionObserver winConditionObserver,
            Player player, 
            EnemySpawner enemySpawner)
        {
            _gameState = GameState.Intro;

            _cover = cover;
            _introScreen = introScreen;
            _loseScreen = loseScreen;
            _winScreen = winScreen;

            _inputHandler = inputHandler;
            _inputHandler.Click += OnClick;
            
            _winConditionObserver = winConditionObserver;
            _winConditionObserver.CloseWinCallback += OnPlayerCloseToDestination;  
            _winConditionObserver.WinCallback += OnPlayerReachedDestination;

            _player = player;
            _player.Died += OnPlayerDied;

            _enemySpawner = enemySpawner;
        }
        
        public void Dispose()
        {
            _inputHandler.Click -= OnClick;
            
            _winConditionObserver.CloseWinCallback -= OnPlayerCloseToDestination;  
            _winConditionObserver.WinCallback -= OnPlayerReachedDestination;

            _player.Died -= OnPlayerDied;
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
        
        private void OnPlayerDied()
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

        private void OnPlayerCloseToDestination()
        {
            _enemySpawner.DisableSpawn();
        }

        private void OnPlayerReachedDestination()
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
            _player.ResetState();
            _enemySpawner.ResetState();
            _enemySpawner.EnableSpawn();
            
            _loseScreen.HideInstant();
            _winScreen.HideInstant();
            
            _winConditionObserver.ResetState();
            
            _introScreen.ShowInstant();
            _gameState = GameState.Intro;
        }
    }
}