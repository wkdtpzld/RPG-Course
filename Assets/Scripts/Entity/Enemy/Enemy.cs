using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }
    public BoxCollider2D cd { get; private set; }

    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 sturnDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    public float detectDistance;
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    public float attackViewDistance;
    [HideInInspector] public float lastTimeAttacked;


    override protected void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();

        cd = GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }


    public virtual void FreezeTimer(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    public virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTimer(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTimer(false);
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    #region Counter Attack
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    #endregion

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationTrigger();

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }
}
