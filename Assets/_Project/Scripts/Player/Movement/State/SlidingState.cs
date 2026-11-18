using UnityEngine;

namespace _Project.Scripts.Player.State
{
    public class SlidingState : GroundedState
    {
        protected override PhysicsParams StatePhysics => Config.slideParams;

        private readonly float _originalColliderHeight;
        private readonly Vector3 _originalColliderCenter;

        private float _slideStartTime;

        public SlidingState(PlayerStateMachine player) : base(player)
        {
            _originalColliderHeight = Player.Collider.height;
            _originalColliderCenter = Player.Collider.center;
        }

        public override void Enter()
        {
            foreach (var t in Config.rotateOnSlideTransforms)
            {
                float targetXRotation = t.localEulerAngles.x - Config.slideRotationXOffset;
                t.localRotation = Quaternion.Euler(targetXRotation, t.localEulerAngles.y, t.localEulerAngles.z);
            }

            foreach (var t in Config.moveOnSlideTransforms)
            {
                float targetYPosition = t.localPosition.y - Config.slidePositionYOffset;
                t.localPosition = new Vector3(t.localPosition.x, targetYPosition, t.localPosition.z);
            }

            // // Shrink the collider so the player can slide under obstacles
            Player.Collider.height = _originalColliderHeight * 0.5f; // Cut height in half
            Player.Collider.center =
                new Vector3(0, Player.Collider.height / 2, 0); // Move center down so feet stay on floor

            _slideStartTime = Time.time;
        }

        public override void Exit()
        {
            foreach (var t in Config.rotateOnSlideTransforms)
            {
                float targetXRotation = t.localEulerAngles.x + Config.slideRotationXOffset;
                t.localRotation = Quaternion.Euler(targetXRotation, t.localEulerAngles.y, t.localEulerAngles.z);
            }

            foreach (var t in Config.moveOnSlideTransforms)
            {
                float targetYPosition = t.localPosition.y + Config.slidePositionYOffset;
                t.localPosition = new Vector3(t.localPosition.x, targetYPosition, t.localPosition.z);
            }

            // Restore original collider size
            Player.Collider.height = _originalColliderHeight;
            Player.Collider.center = _originalColliderCenter;
        }

        public override void LogicUpdate()
        {
            // base.LogicUpdate();
            //
            // if (!Player.InputHandler.SlideInput)
            //     Player.ChangeState(Player.WalkingState);

            // Stop sliding if the button is released AND the minimum slide duration has passed
            if (!Player.InputHandler.SlideInput && (Time.time - _slideStartTime > Config.slideDuration) ||
                Player.Rb.linearVelocity.magnitude < Config.minVelocity)
            {
                Player.ChangeState(Player.WalkingState);
            }
        }
    }
}