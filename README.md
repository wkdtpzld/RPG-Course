# RPG-Course

## 목차

1. FSM 패턴


## 1. FSM 패턴

하나의 시스템이 유한한 상태들 중 하나의 상태에만 존재하며, 특정 조건이 충족될 때 **다른 상태로 전이(Transition)**되는 모델

### 1.1 FSM의 주요 구성 요소

#### State (상태):

시스템이 가질 수 있는 유한한 상태 중 하나.
예: Idle, Move, Jump, Attack

#### Transition (전이):

특정 조건이 충족될 때 한 상태에서 다른 상태로 이동.
예: 플레이어가 키를 누르면 Idle → Move

#### Event (이벤트):

상태 전환을 트리거하는 행동이나 조건.
예: 사용자의 입력, 타이머, 충돌 감지 등.

#### Initial State (초기 상태):

FSM이 시작할 때 시스템의 초기 상태.

```c#
public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}

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

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }
}
```

만약에 ChangeState 로 Idle 이 아닌 Move 라는 상태값으로 바꾼다면 Player -> PlayerMachine -> PlayerState -> PlayerIdle(종료) -> PlayerMove(시작) 으로 상태값을 바꿀수 있게 된다.

상태 전환 조건과 로직이 체계적으로 분리되어 확장성이 증가

새로운 상태를 추가하거나 전환 조건을 변경하기 쉬워져 디버깅에 용이

현재 상태를 추적하기 쉬워 상태 전환 문제를 찾기 편리.
