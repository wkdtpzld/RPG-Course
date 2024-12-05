using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    private Player player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }
}
