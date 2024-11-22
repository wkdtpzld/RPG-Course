using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Sword_Skill_Controller : MonoBehaviour
{
    [Header("Sword Info")]
    [SerializeField] private float returnSpeed = 12f;
    public int amountOfBounce = 4;
    public float bounceSpeed;
    public bool isBouncing = true;
    public List<Transform> enemyTarget;

    [Space]

    private int targetIndex;
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

    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;

        animator.SetBool("Rotation", true);
    }

    public void ReturnSword()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        isReturning = true;
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

    private void handleSortEnemy()
    {
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

        enemyTarget.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(transform.position, a.position);
            float distanceB = Vector2.Distance(transform.position, b.position);
            return distanceB.CompareTo(distanceA);
        });
    }

    private void StuckInfo(Collider2D collision)
    {
        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0) return;

        transform.parent = collision.transform;
        animator.SetBool("Rotation", false);
    }
}
