namespace Project
{
    using UnityEngine;

    public class EnemyBrain
    {
        private enum State { Idle, Wander, Chase, Attack, Dead }

        private State _state;

        private readonly Enemy _body;
        private readonly EnemyConfig _config;
        private readonly Player _player;
        private readonly Timer _idleTimer;

        private bool _canAggro = false;

        private Vector3 _wanderAnchor;
        
        public EnemyBrain(Enemy body, EnemyConfig config, Player player)
        {
            _body = body;
            _config = config;
            _player = player;
            _idleTimer = new Timer();
        }
        
        public void EnableAggro()
        {
            _canAggro = true;
        }

        public void DisableAggro()
        {
            _canAggro = false;
            
            if (_state != State.Dead)
            {
                EnterWander();
            }
        }
        
        public void Spawn(Vector3 position)
        {
            _canAggro = false;
            _wanderAnchor = position;
            
            EnterWander();
        }
        
        public void Stop()
        {
            _state = State.Dead;
        }

        public void Tick(float deltaTime)
        {
            switch (_state)
            {
                case State.Idle:
                    UpdateIdle(deltaTime);
                    break;
                case State.Wander:
                    UpdateWander();
                    break;
                case State.Chase:
                    UpdateChase();
                    break;
                case State.Attack:
                    UpdateAttack();
                    break;
                case State.Dead:
                    break;
            }
        }
        
        private void EnterIdle()
        {
            _state = State.Idle;
            
            _idleTimer.Start(Random.Range(_config.WanderWaitRange.x, _config.WanderWaitRange.y));
            _body.Idle();
        }
        
        private void EnterWander()
        {
            _state = State.Wander;
            
            Vector2 offset = Random.insideUnitCircle * _config.WanderRadius;
            _body.WalkTo(_wanderAnchor + new Vector3(offset.x, 0, offset.y));
        }

        private void EnterChase()
        {
            _state = State.Chase;
        }

        private void EnterAttack()
        {
            _state = State.Attack;
            _body.Attack();
        }

        private void UpdateIdle(float deltaTime)
        {
            if (TryAggro())
            {
                return;
            }
            
            _idleTimer.Tick(deltaTime);
            if (_idleTimer.IsStopped)
            {
                EnterWander();
            }
        }
        
        private void UpdateWander()
        {
            if (TryAggro())
            {
                return;
            }
            
            if (_body.HasReachedDestination())
            {
                EnterIdle();
            }
        }

        private bool TryAggro()
        {
            if (_canAggro && IsPlayerInVision())
            {
                EnterChase();
                return true;
            }

            return false;
        }

        private void UpdateChase()
        {
            if (IsPlayerInAttackDistance())
            {
                EnterAttack();
                return;
            }
            
            _body.RunTo(_player.Position);
        }
        
        private void UpdateAttack()
        {
            _body.RunTo(_player.Position);
        }
        
        public void OnAttackEnded()
        {
            if (_state == State.Attack)
            {
                EnterChase();
            }
        }

        public void OnTakeDamage()
        {
            OnAttackEnded();
        }
        
        private bool IsPlayerInVision()
        {
            return (_body.Position - _player.Position).sqrMagnitude 
                   < _config.VisionDistance * _config.VisionDistance;
        }
        
        private bool IsPlayerInAttackDistance()
        {
            return (_body.Position - _player.Position).sqrMagnitude 
                   < _config.AttackDistance * _config.AttackDistance;
        }
    }
}