using _Project.Scripts.Player.State;
using UnityEngine;

public class AirborneState : ContinuousState

{
    protected override PhysicsParams StatePhysics => Config.airParams;

    private int _wallJumpCount = 0;

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
            ClearJumpCooldowns();
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

    public override void OnJumpInput()
    {
        if (!CanWallJump()) return;
        Vector3 wallNormal;
        if (!TryGetWallNormal(out wallNormal)) return;

        ClearVelocity(false, true, false);
        ApplyImpulse((wallNormal * Config.wallJumpSideForce) + (Vector3.up * Config.wallJumpUpForce));

        ClearJumpCooldowns();
        Debug.Log("Wall Jumped!");
    }

    private bool CanWallJump()
    {
        return _wallJumpCount <= Config.wallJumpLimit;
    }

    private void ClearJumpCooldowns()
    {
        _wallJumpCount = 0;
        Player.DashCharges.ClearAirDash();
    }
}