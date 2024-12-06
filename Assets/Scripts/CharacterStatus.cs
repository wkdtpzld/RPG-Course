using UnityEditor.ShaderGraph.Internal;
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

    [Header("Offensive status")]
    public Status critChance;
    public Status damage;
    public Status critPower;
    public Status magicResistance;

    [Header("Magic status")]
    public Status fireDamge;
    public Status iceDamage;
    public Status lightingDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;


    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }

    protected virtual void Update()
    {

        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        if (igniteDamageTimer < 0 && isIgnited)
        {
            Debug.Log("Take burn Damage" + igniteDamage);

            currentHealth -= igniteDamage;
            if (currentHealth < 0)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStatus _targetStatus)
    {
        if (CanAvoidAttack(_targetStatus)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCritcal())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStatus, totalDamage);
        // _targetStatus.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStatus);
    }

    public virtual void DoMagicalDamage(CharacterStatus _targetStatus)
    {
        int _fireDamage = fireDamge.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(_targetStatus, totalMagicalDamage);

        _targetStatus.TakeDamage(totalMagicalDamage);

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightDamage;
        bool canApplyShock = _lightDamage > _iceDamage && _lightDamage > _fireDamage;

        if (Mathf.Max(_fireDamage, _iceDamage, _lightDamage) <= 0) return;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _lightDamage > 0)
            {
                canApplyShock = true;
                _targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            _targetStatus.SetUpIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }

        _targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
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

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CheckTargetArmor(CharacterStatus _targetStatus, int totalDamage)
    {
        if (_targetStatus.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStatus.armor.GetValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStatus.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);

        return totalDamage;
    }

    private bool CanCritcal()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked) return;

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 4;
        }

        if (_chill)
        {
            chilledTimer = 2;
            isChilled = _chill;
        }
        if (_shock)
        {
            shockedTimer = 2;
            isShocked = _shock;
        }

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
    }

    public void SetUpIgniteDamage(int _damage) => igniteDamage = _damage;

    private int CheckTargetResistance(CharacterStatus _targetStatus, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStatus.magicResistance.GetValue() + (_targetStatus.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);

        return totalMagicalDamage;
    }
}
