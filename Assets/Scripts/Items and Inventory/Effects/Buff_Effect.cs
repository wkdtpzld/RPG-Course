using UnityEngine;

public enum StatusType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}

[CreateAssetMenu(fileName = "New Item Effect", menuName = "Data/Item effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStatus status;
    [SerializeField] private StatusType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        status = PlayerManager.instance.player.GetComponent<PlayerStatus>();

        status.IncreaseStatusBy(buffAmount, buffDuration, StatusToModify());
    }

    private Status StatusToModify()
    {
        if (buffType == StatusType.strength) return status.strength;
        if (buffType == StatusType.agility) return status.agility;
        if (buffType == StatusType.intelligence) return status.intelligence;
        if (buffType == StatusType.vitality) return status.vitality;
        if (buffType == StatusType.damage) return status.damage;
        if (buffType == StatusType.critChance) return status.critChance;
        if (buffType == StatusType.critPower) return status.critPower;
        if (buffType == StatusType.maxHealth) return status.maxHealth;
        if (buffType == StatusType.armor) return status.armor;
        if (buffType == StatusType.evasion) return status.evasion;
        if (buffType == StatusType.magicResistance) return status.magicResistance;
        if (buffType == StatusType.fireDamage) return status.fireDamge;
        if (buffType == StatusType.iceDamage) return status.iceDamage;
        if (buffType == StatusType.lightingDamage) return status.lightingDamage;

        return null;
    }
}