using _Project.Scripts.Player.State;
using UnityEngine;

public class AirborneState : ContinuousState

{
    protected override PhysicsParams StatePhysics => Config.airParams;

    private int _dashCount = 0;

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
        {
            Player.DashCharges.ClearAirDash();
            Player.ChangeState(Player.WalkingState);
        }
    }

    private void Jump()
    {
        ClearVelocity(false, true, false);
        ApplyImpulse(new Vector3(0, Config.jumpForce, 0));
    }

    public override void OnDashInput()
    {
        if (!Player.DashCharges.TryAirDash()) return;

        Player.ChangeState(Player.DashingState);
    }
}