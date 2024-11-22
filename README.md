![image](https://github.com/user-attachments/assets/34a5ba50-7939-4774-a204-edd0fb6207ee)# RPG-Course

## 목차

개선 사항.
1. 검 던지기 기능 리팩토링.

주 학습내용.

1. FSM 패턴
2. Sub-state Machine
3. 코루틴
4. Sorting Layer
5. Cinematic Studio (Camera)
6. 싱글톤 패턴

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

## 4. Sorting Layer

간단하게 말해서 CSS ZIndex 같은 느낌으로 이해했는데.. 이게 맞나 싶다.

![image](https://github.com/user-attachments/assets/9feac5a4-03e5-44fa-bee3-8f6ca0fda304)

각 Sprite Renderer 에서

![image](https://github.com/user-attachments/assets/9c82f994-4caa-4cfd-9ad4-f59f0580fe09)

Sorting Layer 에 맞는 순으로 체크하면 ZIndex 설정 가능하다.

![image](https://github.com/user-attachments/assets/97802376-dbd6-4b65-9131-0443a57387e1)

## 5. Cinematic Studio Camera

![image](https://github.com/user-attachments/assets/8f8c0f8b-270f-46e2-bf02-ba18b24c95fc)

패키지 다운로드 후 Cinematic Camera 오브젝트를 생성하면 자동으로 기본 세팅이 이루어지는데

이렇게 편해도 되나싶다. 구동원리도 모르겠고 그냥 툴을 쓰는듯이 카메라 무빙이 세팅이 가능한데. 일단 쓴다고는 하니까.. 써봤는데 너~무 편하다.

![image](https://github.com/user-attachments/assets/6390a5d5-8566-4177-bed2-eecd20f22df3)

각각에 옵션들은 너무 많다보니 하나하나 언급하기 어렵지만.

카메라가 움직이는 속도. 최소범위 최대범위, 부드러운 정도, 카메라 위치, 크기, 등등 세세한 움직임이 가능하다.

## 6. 싱글톤 패턴

싱글톤 패턴은 프로젝트 어디서든 같은 인스턴스를 참조할 수 있도록 설계된 디자인 패턴입니다.

밑과 같은 Manager를 만들어 둔다면.
전역 어디서든 Player 라는 인스턴스에 접근이 가능해집니다.

초보자들에게 쉽게 추천되는 방식이라고 합니다. 저도 지금 당장 싱글톤이 나왔지만 검색해보니 다른 방식도 많이 사용한다고 합니다.

클래스간 결합도가 너무 많아지며. 테스트가 어렵고 전역 상태관리가 힘들다고 합니다.

```c#

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
}

```


## 개선사항 1. 검 던지기 기능 리팩토링

```c#
    void Update()
    {
        ...
        if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
        {
            targetIndex++;
            amountOfBounce--;

            if (amountOfBounce <= 0)
            {
                isBouncing = false;
                isReturning = true;
            }

            if (targetIndex >= enemyTarget.Count)
            {
                targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ...
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
    }
    
```

colliders 를 리스트형으로 가져올 수 있는 함수이다.

여기서 타입 <Enemy> 라는 타입을 특정하여 기존의 List 에 Add 를 하여 사용하면 쉽게 필터링이 가능했다.

다만 여기서 조금의 의문점이 들었다.

1. OverlapCircleAll 로 모든 오브젝트를 읽는다면 만약 수많은 오브젝트가 있다면 이 코드는 과연 효율적인가?
2. if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f) 의 targetIndex 의 기본값은 0 이지만. 시작부분의 Index 가 0 이라면 어떻게 할 것인지? OverlapCircleAll 의 리스트 정렬 방식은 어떻게 되어있는지?


### 해결방안 및 과정

1. 기존의 코드로 Debug.Log(count) 를 실시했을 때 몇 가지의 오브젝트 수가 감지되었을까?

```c#
Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

Debug.Log(colliders.Length); // 5개 ~ 4개
```

지금은 괜찮지만 오브젝트 수가 많아지면 조금 비효율적일 것 같기도 하고.. 뭔가 이래도 상관이 없을것 같기도 한데.. 만약에 다른 방식으로 한다면 어떻게 하면 좋을까? 싶어서 구현한 코드이다.


```c#
    ContactFilter2D filter = new ContactFilter2D();
    filter.SetLayerMask(LayerMask.GetMask("Enemy"));

    Collider2D[] colliders = new Collider2D[10];
    cd.radius = 10;
    int count = cd.Overlap(filter, colliders);

    for (int i = 0; i < count; i++)
    {
        if (colliders[i].GetComponent<Enemy>() != null)
        {
            enemyTarget.Add(colliders[i].transform);
        }
    }
```

filter.SetLayerMask(LayerMask.GetMask("Enemy")); <- 레이어 마스크를 기준으로 필터를 걸어서 최대 10개의 적을 캐치할 수 있도록 하였고, count 수 만큼의 적을 enemyTarget 에 추가하였다.


2. [targetIndex] OverlapCircleAll 의 정렬 기준은 무엇인가?

OverlapCircleAll 는 Z축을 기반으로 리스트의 정렬을 정한다.

그러므로 따로 Sort 함수를 만들어 정렬을 해줘야 하므로 다음과 같은 코드를 추가하였다.

```c#
    enemyTarget.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(transform.position, a.position);
            float distanceB = Vector2.Distance(transform.position, b.position);
            return distanceB.CompareTo(distanceA);
        });
```

늘 보던 익숙한 그 Sort 함수를 추가.

### 결과물


https://github.com/user-attachments/assets/4db9ec18-4a08-49e9-bdc3-a4e26959f9c4

