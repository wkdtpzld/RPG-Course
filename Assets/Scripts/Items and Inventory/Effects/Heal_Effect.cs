using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect", menuName = "Data/Item effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();

        int healAmount = Mathf.RoundToInt(playerStatus.GetMaxHealthValue() * healPercent);

        playerStatus.IncreaseHealthBy(healAmount);
    }
}
