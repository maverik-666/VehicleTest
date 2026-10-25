namespace Project
{
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField]
        private Enemy _enemyPf;

        public Enemy Create(Vector3 position)
        {
            var enemy = Instantiate(_enemyPf,  position, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
            return enemy;
        }

        public void Dispose(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}