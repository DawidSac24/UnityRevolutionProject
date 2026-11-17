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
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void LogicUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}