namespace Project
{
    using System;
    using System.Collections.Generic;
    using Reflex.Attributes;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Enemy _enemyPf;

        [Range(0, 1)]
        [SerializeField]
        private float _spawnRate;

        [SerializeField]
        private float _amplitude;
        
        private Player _player;

        private List<Enemy> _enemies = new List<Enemy>();
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
            
        }
    }
}