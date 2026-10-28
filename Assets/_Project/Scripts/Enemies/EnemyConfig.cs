namespace Project
{
    using UnityEngine;

    [CreateAssetMenu]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Stats")]
        public int Health = 100;
        public float Radius = 0.5f;
        public float WalkingSpeed = 0.75f;
        public float RunningSpeed = 5;
        public float VisionDistance = 20;
        public float AttackDistance = 5;
        public int AttackDamage = 15;
        public float WanderRadius = 3;
        public Vector2 WanderWaitRange = new Vector2(1, 3);
        
        [Header("Creation")]
        public int RegistryCapacity = 50;
        public int PoolCapacity = 50;
        public int PoolMaxSize = 200;
    }
}