namespace Project
{
    using System;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class EnemySkinRenderer : MonoBehaviour
    {
        private static readonly int TakeDamageId = Animator.StringToHash("TakeDamage");
        private static readonly int AttackId = Animator.StringToHash("Attack");

        [SerializeField]
        private ParticleSystem _deathVfx;
        
        private Action _hitLanded;
        private Animator _animator;
        
        public void Init(Action hitLanded)
        {
            _hitLanded = hitLanded;
            
            _animator = GetComponent<Animator>();
            _animator.SetLayerWeight(1, 0);
        }
        
        public void AnimateTakeDamage()
        {
            _animator.SetLayerWeight(1, 1);
            _animator.Play(TakeDamageId, 1, 0);
        }
        
        public void AnimateAttack()
        {
            _animator.SetLayerWeight(1, 1);
            _animator.Play(AttackId, 1, 0);
        }

        public void OnHitLanded()
        {
            _hitLanded?.Invoke();
        }

        public void OnTakeDamageAnimationEnded()
        {
            _animator.SetLayerWeight(1, 0);
        }
        
        public void OnAttackAnimationEnded()
        {
            _animator.SetLayerWeight(1, 0);
        }
        
        public void AnimateDeath()
        {
            //_deathVfx.Play();
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            gameObject.SetActive(true);
        }
    }
}