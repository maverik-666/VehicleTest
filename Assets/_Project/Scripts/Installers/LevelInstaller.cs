namespace Project
{
    using Reflex.Core;
    using Reflex.Enums;
    using UnityEngine;
    using Resolution = Reflex.Enums.Resolution;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class LevelInstaller : MonoBehaviour, IInstaller
    {
        [Header("Utils")]
        [SerializeField]
        private Camera _camera;
        
        [SerializeField]
        private InputHandler _inputHandler;
        
        [Header("UI")]
        [SerializeField]
        private Cover _cover;
        
        [SerializeField]
        private IntroScreen _introScreen;

        [SerializeField]
        private LoseScreen _loseScreen;
        
        [SerializeField]
        private WinScreen _winScreen;
        
        [SerializeField]
        private ProgressBar _progressBar;
        
        [Header("Gameplay")]
        [SerializeField]
        private Player _player;
        
        [SerializeField]
        private WinConditionObserver _winConditionObserver;

        [Header("Enemy")]
        [SerializeField]
        private EnemyConfig _enemyConfig;
        
        [SerializeField]
        private EnemyFactory _enemyFactory;

        [SerializeField]
        private EnemySpawner _enemySpawner;

        [Header("Projectile")]
        [SerializeField]
        private ProjectileConfig _projectileConfig;
        
        [SerializeField]
        private ProjectileFactory _projectileFactory;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterValue(_camera);
            containerBuilder.RegisterValue(_inputHandler);
            
            containerBuilder.RegisterValue(_cover);
            containerBuilder.RegisterValue(_introScreen);
            containerBuilder.RegisterValue(_loseScreen);
            containerBuilder.RegisterValue(_winScreen);

            containerBuilder.RegisterValue(_progressBar);
            
            containerBuilder.RegisterValue(_player);
            containerBuilder.RegisterValue(_winConditionObserver);

            containerBuilder.RegisterValue(_enemyConfig);
            containerBuilder.RegisterType(typeof(EnemyRegistry), Lifetime.Scoped, Resolution.Lazy);
            containerBuilder.RegisterValue(_enemyFactory);
            containerBuilder.RegisterValue(_enemySpawner);

            containerBuilder.RegisterValue(_projectileConfig);
            containerBuilder.RegisterValue(_projectileFactory);
            
            containerBuilder.RegisterType(typeof(GameFlow), Lifetime.Scoped, Resolution.Eager);
        }
    }
}