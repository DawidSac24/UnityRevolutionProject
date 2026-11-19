using _Project.Scripts.Player.State;

public abstract class GroundedState : ContinuousState
{
    protected GroundedState(PlayerStateMachine player) : base(player)
    {
    }

    public override void LogicUpdate()
    {
        if (!IsGrounded())
        {
            Player.ChangeState(Player.AirborneState);
        }
    }


    public override void OnJumpInput()
    {
        Player.ChangeState(Player.AirborneState);
    }
}