using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [Header("Bounce Info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Pierce Info")]
    private float pierceAmount;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDutation;
    public float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    public float hitTimer;
    private float hitCooldown;
    private float spinDirection;

    [Header("Sword State")]
    private SwordType swordType;

    [Space]
    [SerializeField] private float returnSpeed;
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player, SwordType _swordType)
    {
        player = _player;
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;
        swordType = _swordType;

        if (pierceAmount <= 0)
            animator.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1);
    }

    public void SetUpBounce(bool _isBouncing, int _amountOfBounce)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;

        enemyTarget = new List<Transform>();
    }

    public void SetUpPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void ReturnSword()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        isReturning = true;
    }

    public void SetUpSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDutation = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.linearVelocity;
        }

        if (isReturning)
        {
            handleReturnSword();
        }

        if (isBouncing && enemyTarget.Count > 0)
        {
            handleBouncingSword();
        }

        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopSpinning();
            }

            if (wasStopped)
            {
                SpinAttack();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) return;

        if (collision.GetComponent<Enemy>() != null && isBouncing && enemyTarget.Count == 0)
        {
            handleSortEnemy();
        }

        StuckInfo(collision);
    }

    private void SpinAttack()
    {
        spinTimer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

        if (spinTimer < 0)
        {
            isReturning = true;
            isSpinning = false;
        }

        hitTimer -= Time.deltaTime;
        if (hitTimer < 0)
        {
            hitTimer = hitCooldown;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    hit.GetComponent<Enemy>().Damage();
                }
            }
        }
    }

    private void handleReturnSword()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
        animator.SetBool("Rotation", true);

        if (Vector2.Distance(transform.position, player.transform.position) < 1)
        {
            animator.SetBool("Rotation", false);
            player.CatchTheSword();
        }
    }

    private void handleBouncingSword()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
        {
            enemyTarget[targetIndex].GetComponent<Enemy>()?.Damage();
            targetIndex++;
            bounceAmount--;

            if (bounceAmount <= 0)
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

    private void handleSortEnemy()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Enemy"));

        Collider2D[] colliders = new Collider2D[10];
        cd.radius = 6;
        int count = cd.Overlap(filter, colliders);

        for (int i = 0; i < count; i++)
        {
            if (colliders[i].GetComponent<Enemy>() != null)
            {
                enemyTarget.Add(colliders[i].transform);
            }
        }

        enemyTarget.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(transform.position, a.position);
            float distanceB = Vector2.Distance(transform.position, b.position);
            return distanceB.CompareTo(distanceA);
        });
    }

    private void StuckInfo(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>()?.Damage();
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
            if (!wasStopped)
            {
                StopSpinning();
            }
            return;
        }
        if (collision.GetComponent<Enemy>() != null)
        {
            if (swordType == SwordType.Regular || swordType == SwordType.Pierce)
            {
                Debug.Log("Damage");
                collision.GetComponent<Enemy>()?.Damage();
            }
        }
        cd.enabled = false;
        canRotate = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0) return;

        transform.parent = collision.transform;
        animator.SetBool("Rotation", false);
    }

    private void StopSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDutation;
    }
}
