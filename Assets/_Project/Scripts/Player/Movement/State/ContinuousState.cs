using UnityEngine;

namespace _Project.Scripts.Player.State
{
    public abstract class ContinuousState : PlayerState
    {
        protected ContinuousState(PlayerStateMachine sm) : base(sm)
        {
        }

        protected abstract PhysicsParams StatePhysics { get; }


        public override void PhysicsUpdate()
        {
            if (Player.Rb.isKinematic) return;

            Vector2 moveVector = Player.InputHandler.MoveInput;
            Vector3 forward = Player.transform.TransformDirection(Vector3.forward);
            Vector3 right = Player.transform.TransformDirection(Vector3.right);

            // Calculate Desired Force
            Vector3 moveForce = (forward * moveVector.y + right * moveVector.x).normalized *
                                (StatePhysics.targetMoveSpeed * StatePhysics.movementMultiplier);
            Vector3 currentHorizontalVelocity =
                new Vector3(Player.Rb.linearVelocity.x, 0f, Player.Rb.linearVelocity.z);

            // No Input (Idle Friction)
            if (moveVector == Vector2.zero)
            {
                if (StatePhysics.idleFriction > 0f)
                {
                    currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, Vector3.zero,
                        StatePhysics.idleFriction * Time.fixedDeltaTime);
                    Player.Rb.linearVelocity = new Vector3(currentHorizontalVelocity.x,
                        Player.Rb.linearVelocity.y, currentHorizontalVelocity.z);
                }
            }
            // Player Giving Input
            else
            {
                float dot = Vector3.Dot(moveForce.normalized, currentHorizontalVelocity.normalized);

                // Fighting Momentum (Turnaround)
                if (StatePhysics.turnaroundFriction > 0f && dot < 0)
                {
                    currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, Vector3.zero,
                        StatePhysics.turnaroundFriction * Time.fixedDeltaTime);
                    Player.Rb.linearVelocity = new Vector3(currentHorizontalVelocity.x,
                        Player.Rb.linearVelocity.y, currentHorizontalVelocity.z);
                }

                // OVER THE LIMIT
                if (currentHorizontalVelocity.magnitude > StatePhysics.targetMoveSpeed)
                {
                    // Only apply force if braking/steering
                    if (dot <= 0)
                    {
                        Player.Rb.AddForce(moveForce, ForceMode.Force);
                    }

                    // Bleed speed back down to target
                    if (StatePhysics.bleedingFriction > 0f)
                    {
                        Vector3 targetBleedVelocity =
                            currentHorizontalVelocity.normalized * StatePhysics.targetMoveSpeed;
                        Vector3 bledVelocity = Vector3.Lerp(currentHorizontalVelocity, targetBleedVelocity,
                            StatePhysics.bleedingFriction * Time.fixedDeltaTime);
                        Player.Rb.linearVelocity = new Vector3(bledVelocity.x, Player.Rb.linearVelocity.y,
                            bledVelocity.z);
                    }
                }
                // UNDER THE LIMIT
                else
                {
                    Player.Rb.AddForce(moveForce, ForceMode.Force);

                    // Optional Hard Cap (usually only for grounded states)
                    if (StatePhysics.appliesHardCap)
                    {
                        Vector3 newHorizontalVelocity = new Vector3(Player.Rb.linearVelocity.x, 0f,
                            Player.Rb.linearVelocity.z);
                        if (newHorizontalVelocity.magnitude > StatePhysics.targetMoveSpeed)
                        {
                            Vector3 hardCap = newHorizontalVelocity.normalized * StatePhysics.targetMoveSpeed;
                            Player.Rb.linearVelocity =
                                new Vector3(hardCap.x, Player.Rb.linearVelocity.y, hardCap.z);
                        }
                    }
                }
            }
        }
    }
}