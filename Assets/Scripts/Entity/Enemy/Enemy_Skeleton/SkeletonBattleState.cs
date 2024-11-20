using UnityEngine;
using UnityEngine.EventSystems;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy;
    private int moveDirection;
    private Transform player;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animatorBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animatorBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
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
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > enemy.attackViewDistance)
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDirection, rb.linearVelocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
