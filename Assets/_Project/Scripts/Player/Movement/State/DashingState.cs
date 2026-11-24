using _Project.Scripts.Player.State;
using UnityEngine;

public class DashingState : PlayerState
{
    public DashingState(PlayerStateMachine sm) : base(sm)
    {
    }

    public override void Enter()
    {
        Dash();
        Player.ChangeState(Player.LastState);
    }

    public override void Exit()
    {
    }

    public override void LogicUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
    }

    private void Dash()
    {
        Vector3 dashDir = GetMovementDirection();
        if (dashDir == Vector3.zero)
        {
            dashDir = Player.transform.forward;
        }

        ApplyImpulse(dashDir * Config.dashForce);
    }
}