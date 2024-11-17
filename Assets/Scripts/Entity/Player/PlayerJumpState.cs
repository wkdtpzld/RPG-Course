using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName) : base(_player, _stateMachine, _animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);
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
            player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);
        }

        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}