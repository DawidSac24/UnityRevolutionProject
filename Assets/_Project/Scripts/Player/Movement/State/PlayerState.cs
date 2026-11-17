namespace _Project.Scripts.Player.State
{
    public abstract class PlayerState
    {
        protected PlayerStateMachine _player;
        protected PlayerMovementConfig _movementConfig;

        public PlayerState(PlayerStateMachine stateMachine, PlayerMovementConfig movementConfig)
        {
            _player = stateMachine;
            _movementConfig = movementConfig;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void LogicUpdate(float deltaTime);
        public abstract void PhysicsUpdate(float deltaTime);
    }
}