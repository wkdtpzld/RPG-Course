using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float precentageModifier = .4f;
    protected override void Start()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(critChance);
        Modify(damage);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamge);
        Modify(iceDamage);
        Modify(lightingDamage);

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void Modify(Status _status)
    {
        for (int i = 0; i < level; i++)
        {
            float modifer = _status.GetValue() * precentageModifier;
            _status.AddModifier(Mathf.RoundToInt(modifer));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        myDropSystem.GenerateDrop();
    }
}
