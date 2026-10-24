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
            containerBuilder.RegisterFactory(_ => GetComponentInChildren<Player>(true),
                Lifetime.Scoped, Resolution.Eager);
        }
    }
}