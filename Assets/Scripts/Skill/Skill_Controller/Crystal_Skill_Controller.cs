using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float crystalExistTimer;
    private bool canMoveToEnemy;
    private float moveSpped;
    private bool canExplode;

    private bool canGrow;
    private bool canMove;
    private float growSpeed = 5;
    private Transform closestEnemy;
    [SerializeField] private LayerMask whatIsEnemy;


    public void SetUpCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpped = _moveSpeed;
        closestEnemy = _closestEnemy;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpped * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestEnemy.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    public void SelfDestory() => Destroy(gameObject);

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestory();
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
            closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
    }
}
