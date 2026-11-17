using UnityEngine;

namespace _Project.Scripts.Player.State
{
    public class WalkingState : GroundedState
    {
        protected override PhysicsParams StatePhysics => Config.walkParams;

        public WalkingState(PlayerStateMachine player) : base(player)
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
            base.LogicUpdate();
            
            if (Player.InputHandler.SlideInput)
            {
                Player.CurrentState = Player.SlidingState;
            }
        }
    }
}