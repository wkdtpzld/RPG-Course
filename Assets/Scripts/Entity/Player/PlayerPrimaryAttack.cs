using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName) : base(_player, _stateMachine, _animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0; // 공격시 방향이 이상하게 가는 버그를 고치기 위함의 코드. (xInput 초기화)

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        float attackDir = player.facingDir;
        if (xInput != 0)
        {
            attackDir = xInput;
        }

        player.animator.SetInteger("ComboCounter", comboCounter);
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(.15f));

        comboCounter++;
        lastTimeAttacked = Time.time;
    }
}
