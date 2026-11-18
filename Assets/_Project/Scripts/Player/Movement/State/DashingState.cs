using UnityEngine;

namespace _Project.Scripts.Player.State
{
    public class DashingState : PlayerState
    {
        public DashingState(PlayerStateMachine sm) : base(sm)
        {
        }

        public override void Enter()
        {
            Dash();
            Player.ChangeState(Player.WalkingState);
        }

        public override void Exit()
        {
        }

        public override void LogicUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
        }

        private void Dash()
        {
            Vector3 dashDir = GetMovementDirection();
            if (dashDir == Vector3.zero)
            {
                dashDir = Player.transform.forward;
            }

            ApplyImpulse(dashDir * Config.dashForce);
        }
    }
}