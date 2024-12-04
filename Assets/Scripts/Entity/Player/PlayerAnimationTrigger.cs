using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStatus _target = hit.GetComponent<EnemyStatus>();
                player.status.DoDamage(_target);
            }
        }
    }

    private void ThorwSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
