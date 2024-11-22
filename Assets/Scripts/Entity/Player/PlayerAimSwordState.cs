using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName) : base(_player, _stateMachine, _animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.sword.DotsActive(true);
    }

    public override void Update()
    {
        base.Update();

        player.rb.linearVelocity = new Vector2(0, 0);

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }

        Vector2 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePostion.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < mousePostion.x && player.facingDir == -1)
        {
            player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(.2f));
    }
}
