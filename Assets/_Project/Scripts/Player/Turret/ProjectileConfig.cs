namespace Project
{
    using UnityEngine;

    [CreateAssetMenu]
    public class ProjectileConfig : ScriptableObject
    {
        public int PoolCapacity = 20;
        public int PoolMaxSize = 50;
    }
}