using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    protected bool triggerCalled;
    private string animatorBoolName;

    protected float stateTimer;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animatorBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animatorBoolName = _animatorBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.animator.SetBool(animatorBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animatorBoolName, false);
        enemyBase.AssignLastAnimName(animatorBoolName);
    }

    public virtual void AnimationTrigger()
    {
        triggerCalled = true;
    }
}
