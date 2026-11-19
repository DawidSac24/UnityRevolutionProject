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
            Config = sm.MovementConfig;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();

        public virtual void OnJumpInput()
        {
        }

        public virtual void OnSlideInput()
        {
        }

        public virtual void OnDashInput()
        {
        }


        protected void ClearVelocity(bool clearX, bool clearY, bool clearZ)
        {
            var velocity = Player.Rb.linearVelocity;
            Player.Rb.linearVelocity.Set(clearX ? 0 : velocity.x, clearY ? 0 : velocity.y, clearZ ? 0 : velocity.z);
        }

        protected void ApplyImpulse(Vector3 impulseForce)
        {
            Player.Rb.AddForce(impulseForce, ForceMode.Impulse);
        }

        protected Vector3 GetMovementDirection()
        {
            Vector2 moveVector = Player.InputHandler.MoveInput;
            return (Player.transform.forward * moveVector.y + Player.transform.right * moveVector.x).normalized;
        }
    }
}