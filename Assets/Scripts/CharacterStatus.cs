using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [Header("Major status")]
    public Status strength;
    public Status agility;
    public Status intelligence;
    public Status vitality;

    [Header("Defencive status")]
    public Status maxHealth;
    public Status armor;
    public Status evasion;
    public Status damage;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStatus _targetStatus)
    {
        if (CanAvoidAttack(_targetStatus)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = CheckTargetArmor(_targetStatus, totalDamage);
        _targetStatus.TakeDamage(totalDamage);
    }
    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // throw new NotImplementedException();
    }

    private bool CanAvoidAttack(CharacterStatus _targetStatus)
    {
        int totalEvasion = _targetStatus.evasion.GetValue() + _targetStatus.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CheckTargetArmor(CharacterStatus _targetStatus, int totalDamage)
    {
        totalDamage -= _targetStatus.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);

        return totalDamage;
    }
}
