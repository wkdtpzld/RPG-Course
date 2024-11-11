using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animatorBoolName;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animatorBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animatorBoolName = _animatorBoolName;
    }

    public virtual void Enter()
    {
        Debug.Log("I enter" + animatorBoolName);
    }

    public virtual void Update()
    {
        Debug.Log("I`m in" + animatorBoolName);
    }

    public virtual void Exit()
    {
        Debug.Log("I`m exit" + animatorBoolName);
    }
}
