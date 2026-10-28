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
        private static readonly int MovingId = Animator.StringToHash("Moving");
        private static readonly int RunningId = Animator.StringToHash("Running");

        [SerializeField]
        private ParticleSystem _damageVfx;
        
        [SerializeField]
        private ParticleSystem _deathVfx;

        [SerializeField]
        private Color _takeDamageColor = Color.white;

        [SerializeField]
        private float _flashDuration = 0.15f;
        
        private Renderer _renderer;
        private MaterialFlasher _materialFlasher;
        
        private Action _attackEnded;
        private Animator _animator;
        
        public void Init(Action attackEnded)
        {
            _renderer = GetComponentInChildren<Renderer>(true);
            
            _attackEnded = attackEnded;
            _materialFlasher = new MaterialFlasher(gameObject, _renderer);
            
            _animator = GetComponent<Animator>();
            _animator.SetLayerWeight(1, 0);
        }

        public void AnimateWalk()
        {
            _animator.SetBool(MovingId, true);
            _animator.SetBool(RunningId, false);
        }
        

        public void AnimateIdle()
        {
            _animator.SetBool(MovingId, false);
            _animator.SetBool(RunningId, false);
        }

        public void AnimateRun()
        {
            _animator.SetBool(MovingId, true);
            _animator.SetBool(RunningId, true);
        }
        
        public void AnimateTakeDamage()
        {
            _materialFlasher.FlashColor(_takeDamageColor, _flashDuration);
            _damageVfx.Play();
            
            _animator.SetLayerWeight(1, 1);
            _animator.Play(TakeDamageId, 1, 0);
        }
        
        public void AnimateAttack()
        {
            _animator.SetLayerWeight(1, 1);
            _animator.Play(AttackId, 1, 0);
        }

        public void OnTakeDamageAnimationEnded()
        {
            _animator.SetLayerWeight(1, 0);
        }
        
        public void OnAttackAnimationEnded()
        {
            _attackEnded?.Invoke();
            _animator.SetLayerWeight(1, 0);
        }
        
        public void AnimateDeath()
        {
            _deathVfx.Play();
            _renderer.enabled = false;
            _animator.enabled = false;
        }

        public void ResetState()
        { 
            _renderer.enabled = true;
            _animator.enabled = true;
        }
    }
}