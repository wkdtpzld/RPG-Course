using UnityEngine;

public class EnemyGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;
    public EnemyGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animatorBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animatorBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
