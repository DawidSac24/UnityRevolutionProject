using UnityEngine;

namespace _Project.Scripts.Player.State
{
    public class SlidingState : GroundedState
    {
        protected override PhysicsParams StatePhysics => Config.slideParams;

        public SlidingState(PlayerStateMachine player) : base(player)
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void LogicUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}