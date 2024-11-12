using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName) : base(_player, _stateMachine, _animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.linearVelocity.y);
    }
}
