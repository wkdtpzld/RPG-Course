using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName) : base(_player, _stateMachine, _animatorBoolName)
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(player.blackhole);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            player.StartCoroutine(player.BusyFor(1f));
            stateMachine.ChangeState(player.aimSowrd);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.counterAttack);
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttack);
        }
    }

    private bool HasNoSword()
    {
        if (!player.sword)
            return true;

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
