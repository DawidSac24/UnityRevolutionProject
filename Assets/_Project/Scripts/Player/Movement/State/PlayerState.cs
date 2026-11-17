using UnityEngine;

namespace _Project.Scripts.Player.State
{
    public abstract class PlayerState
    {
        protected PlayerStateMachine Player;
        protected PlayerMovementConfig Config;

        public PlayerState(PlayerStateMachine sm)
        {
            Player = sm;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate(); // Left completely blank/abstract here
    }
}