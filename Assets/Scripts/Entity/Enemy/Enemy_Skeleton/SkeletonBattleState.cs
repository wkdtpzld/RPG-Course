using UnityEngine;
using UnityEngine.EventSystems;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy;
    private Transform player;
    private int moveDirection;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animatorBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animatorBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.position.x > enemy.transform.position.x)
        {
            moveDirection = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDirection = -1;
        }

        if (enemy.IsPlayerDetected())
        {
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                Debug.Log("I ATTACK");
                enemy.ZeroVelocity();
                return;
            }
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDirection, rb.linearVelocity.y);
    }
}
