using Unity.VisualScripting;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;
    private float cloneTimer;
    private float colorLoosingSpeed;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.2f;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private float chanceToDuplicate;
    private int facingDir = 1;
    private Player player;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, float _colorLoosingSpeed, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _chanceToDuplicate, Player _player)
    {
        if (_canAttack)
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 3));
        }
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        colorLoosingSpeed = _colorLoosingSpeed;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        player = _player;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.status.DoDamage(hit.GetComponent<CharacterStatus>());

                if (canDuplicateClone)
                {
                    if (canDuplicateClone)
                    {
                        if (Random.Range(0, 100) < chanceToDuplicate)
                        {
                            SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                        }
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir *= -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}