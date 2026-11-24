using _Project.Scripts.Player.State;
using UnityEngine;

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
        Player.DashCharges.TickRecharge(Time.deltaTime);

        base.LogicUpdate();
    }

    public override void OnSlideInput()
    {
        Player.ChangeState(Player.SlidingState);
    }

    public override void OnDashInput()
    {
        if (!Player.DashCharges.TryGroundDash()) return;

        Player.ChangeState(Player.DashingState);
    }
}