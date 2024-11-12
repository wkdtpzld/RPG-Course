using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName) : base(_player, _stateMachine, _animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.moveSpeed * .8f, rb.linearVelocity.y);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
