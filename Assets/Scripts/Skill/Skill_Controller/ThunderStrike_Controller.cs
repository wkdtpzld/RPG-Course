using System;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStatus targetStatus;
    [SerializeField] private float speed;
    private int damage;

    private Animator animator;
    private bool triggered;

    public void SetUp(int _damage, CharacterStatus _targetStatus)
    {
        damage = _damage;
        targetStatus = _targetStatus;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!targetStatus) return;
        if (triggered) return;
        transform.position = Vector2.MoveTowards(transform.position, targetStatus.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStatus.transform.position;

        if (Vector2.Distance(transform.position, targetStatus.transform.position) < .1f)
        {
            animator.transform.localRotation = Quaternion.identity;
            animator.transform.localPosition = new Vector3(0, .5f);

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);


            Invoke("DamageAndSelfDestory", .2f);

            triggered = true;
            animator.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestory()
    {
        targetStatus.ApplyShock(true);
        targetStatus.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}
