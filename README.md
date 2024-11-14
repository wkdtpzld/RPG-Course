# RPG-Course

## 목차

1. FSM 패턴
2. Sub-state Machine
3. 코루틴


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


## 2. Sub-State Machine

공격 모션의 애니메이션 관리를 위해서 Animator 에서 우클릭 후 Sub-State Machine 을 클릭하여 생성 및 관리

![image](https://github.com/user-attachments/assets/f7ce7071-6a38-4d12-9171-b7149f37b747)

![image](https://github.com/user-attachments/assets/94ffed04-76eb-443b-a724-90674a6d11a6)

더블클릭을 하여 입장하면 아무것도 없겠지만 여기서 공격을 위한 애니메이션 State 관리가 가능하다.

Make Transition 에서 Sub-State 에 관해서 의존성을 넣을지, 안쪽 State 개별에 의존성을 넣을지 선택 가능하다.

playerAttack 은 공통적으로 Attack 이라는 변수가 공통적으로 True 일 경우에만 발동하므로 Attack=true 일 경우의 변수를 안에 다 때려박아서 관리 가능하다.

## 3. 코루틴

유니티에서는 특정 코드가 반복적으로 실행하기 위해서 MonoBehaviour 클래스를 상속받은 클래스에 void Update() 를 실행시켜야 하지만

Update 외에도 실행시켜야 할 경우가 존재한다.

이럴때 코루틴 을 사용하면 해결이 가능하다.

또한 자신이 필요한 순간에만 반복, 필요하지 않을 경우에는 사용하지 않음으로써 자원관리를 효과적으로 할 수 있다.

코루틴의 조건이 있다.

1. IEnumerator 라는 반환형 함수를 작성해야 한다.
2. yield return 이 함수 내부에 존재해야 한다.

```c#
    public bool isBusy { get; private set; }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }
```

Player 의 변수에 public isBusy 라는 변수를 생성하고. isBusy 일 경우에 특정 행동을 하지 못하도록 제한을 걸기 위해서 만들어보았다.
