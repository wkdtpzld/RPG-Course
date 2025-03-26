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

        status.IncreaseStatusBy(buffAmount, buffDuration, status.StatusToModify(buffType));
    }
}