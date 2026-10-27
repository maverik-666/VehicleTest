namespace Project
{
    public class EnemyRegistry : Registry<Enemy>
    {
        public EnemyRegistry(EnemyConfig enemyConfig) : base(enemyConfig.RegistryCapacity) {}
    }
}