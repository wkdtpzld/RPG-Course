using System.Collections;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    private EntityFX fx;

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

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
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
            DecreasehealthBy(igniteDamage);
            if (currentHealth < 0 && !isDead)
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
        _targetStatus.TakeDamage(totalDamage);

        // 임시
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

        AttemptyToApplyAilment(_targetStatus, _fireDamage, _iceDamage, _lightDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreasehealthBy(_damage);

        GetComponent<Entity>().DamageEffect();
        fx.StartCoroutine(fx.FlashFX());

        if (currentHealth < 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void DecreasehealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
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
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.InvokeIgniteFx(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            chilledTimer = ailmentsDuration;
            isChilled = _chill;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.InvokeChillFx(ailmentsDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null) return;
                HitNearestTargetWithShockStrike();
            }
        }

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
    }

    public void SetUpIgniteDamage(int _damage) => igniteDamage = _damage;

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    private int CheckTargetResistance(CharacterStatus _targetStatus, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStatus.magicResistance.GetValue() + (_targetStatus.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);

        return totalMagicalDamage;
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked) return;

        shockedTimer = ailmentsDuration;
        isShocked = _shock;

        fx.InvokeShockFx(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 2)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderStrike_Controller>().SetUp(shockDamage, closestEnemy.GetComponent<CharacterStatus>());
        }
    }

    public void AttemptyToApplyAilment(CharacterStatus _targetStatus, int _fireDamage, int _iceDamage, int _lightDamage)
    {
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

        if (canApplyShock)
        {
            _targetStatus.SetupShockStrikeDamage(Mathf.RoundToInt(_lightDamage * .1f));
        }

        _targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
        {
            currentHealth = GetMaxHealthValue();
        }

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    public virtual void IncreaseStatusBy(int _modifer, float _duration, Status _statusToModify)
    {
        StartCoroutine(StatusModCoroutine(_modifer, _duration, _statusToModify));
    }

    public IEnumerator StatusModCoroutine(int _modifer, float _duration, Status _statusToModify)
    {
        _statusToModify.AddModifier(_modifer);

        yield return new WaitForSeconds(_duration);

        _statusToModify.RemoveModifier(_modifer);
    }
}
