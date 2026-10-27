namespace Project
{
    using UnityEngine;

    [CreateAssetMenu]
    public class EnemyConfig : ScriptableObject
    {
        public int RegistryCapacity = 50;
        public int PoolCapacity = 50;
        public int PoolMaxSize = 200;
    }
}