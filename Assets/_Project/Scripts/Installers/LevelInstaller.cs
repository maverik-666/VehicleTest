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
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            Random.InitState(0);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<Camera>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<InputSystem>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<Player>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => new EnemyRegistry(50), 
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<EnemyFactory>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<EnemySpawner>(true),
                Lifetime.Scoped, Resolution.Eager);

            containerBuilder.RegisterFactory(_ => GetComponentInChildren<Cover>(true),
                Lifetime.Scoped, Resolution.Eager);

            containerBuilder.RegisterFactory(_ => GetComponentInChildren<IntroScreen>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<LoseScreen>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<WinScreen>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<ProgressBar>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<WinConditionObserver>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<GameFlowController>(true),
                Lifetime.Scoped, Resolution.Eager);
            
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<ProjectileFactory>(true),
                Lifetime.Scoped, Resolution.Eager);
        }
    }
}