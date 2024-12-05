using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animatorBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animatorBoolName = _animatorBoolName;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(animatorBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animatorBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
