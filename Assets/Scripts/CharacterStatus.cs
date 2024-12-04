using System;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public Status damage;
    public Status maxHealth;
    public Status strength;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStatus _targetStatus)
    {

        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStatus.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
        {

        }
    }

    protected virtual void Die()
    {
        // throw new NotImplementedException();
    }
}
