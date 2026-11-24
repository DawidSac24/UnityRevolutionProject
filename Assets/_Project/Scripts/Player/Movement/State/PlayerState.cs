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
            Vector3 velocity = Player.Rb.linearVelocity;
            Player.Rb.linearVelocity = new Vector3(
                clearX ? 0f : velocity.x,
                clearY ? 0f : velocity.y,
                clearZ ? 0f : velocity.z
            );
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

        protected bool TryGetWallNormal(out Vector3 wallNormal)
        {
            wallNormal = Vector3.zero;

            CapsuleCollider col = Player.Collider;
            Vector3 center = Player.transform.TransformPoint(col.center);
            float checkRadius = col.radius + Config.wallCheckBuffer; // e.g., 0.3f

            // Distance from the center to the centers of the top/bottom hemispheres
            float halfHeight = Mathf.Max(0f, (col.height * 0.5f) - col.radius);
            Vector3 top = center + Vector3.up * halfHeight;
            Vector3 bottom = center - Vector3.up * halfHeight;

            Collider[] hits = Physics.OverlapCapsule(bottom, top, checkRadius, Config.wallMask);

            if (hits.Length == 0) return false;
            float closestDistSq = float.MaxValue;
            bool foundValidWall = false;

            foreach (Collider hit in hits)
            {
                Vector3 closestPoint = hit.ClosestPoint(center);

                // Find the closest point along the player's vertical spine (between bottom and top)
                // This prevents walls near your feet/head from looking like floors/ceilings
                Vector3 spinePoint = new Vector3(center.x, Mathf.Clamp(closestPoint.y, bottom.y, top.y), center.z);
                Vector3 awayVector = spinePoint - closestPoint;

                // Ignore floors and ceilings
                if (Mathf.Abs(awayVector.normalized.y) > 0.3f) continue;

                float distSq = awayVector.sqrMagnitude;
                if (distSq < closestDistSq && distSq > 0.0001f)
                {
                    closestDistSq = distSq;
                    wallNormal = new Vector3(awayVector.x, 0f, awayVector.z).normalized;
                    foundValidWall = true;
                }
            }

            return foundValidWall;
        }
    }
}