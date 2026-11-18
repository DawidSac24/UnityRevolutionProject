using UnityEngine;

namespace _Project.Scripts.Player.State
{
    public class AirborneState : ContinuousState
    {
        protected override PhysicsParams StatePhysics => Config.airParams;

        public AirborneState(PlayerStateMachine player) : base(player)
        {
        }

        public override void Enter()
        {
            if (IsGrounded())
                Jump();
        }

        public override void Exit()
        {
        }

        public override void LogicUpdate()
        {
            if (IsGrounded())
                Player.ChangeState(Player.WalkingState);
        }

        private void Jump()
        {
            ClearVelocity(false, true, false);
            ApplyImpulse(new Vector3(0, Config.jumpForce, 0));
        }
    }
}