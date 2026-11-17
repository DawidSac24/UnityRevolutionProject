using UnityEngine;

namespace _Project.Scripts.Player.State
{
    public class WalkingState : PlayerState
    {
        public WalkingState(PlayerStateMachine stateMachine, PlayerMovementConfig movementConfig) : base(stateMachine,
            movementConfig)
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void LogicUpdate(float deltaTime)
        {
        }

        public override void PhysicsUpdate(float deltaTime)
        {
            if (_player.rb.isKinematic) return;
            Vector3 forward = _player.transform.TransformDirection(Vector3.forward);
            Vector3 right = _player.transform.TransformDirection(Vector3.right);
        }
    }
}