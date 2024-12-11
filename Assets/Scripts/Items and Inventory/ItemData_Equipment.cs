using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "new Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Major status")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive status")]
    public int critChance;
    public int damage;
    public int critPower;


    [Header("Defencive status")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic status")]
    public int fireDamge;
    public int iceDamage;
    public int lightingDamage;

    public void AddModifiers()
    {
        PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();

        playerStatus.strength.AddModifier(strength);
        playerStatus.agility.AddModifier(agility);
        playerStatus.intelligence.AddModifier(intelligence);
        playerStatus.vitality.AddModifier(vitality);

        playerStatus.maxHealth.AddModifier(maxHealth);
        playerStatus.armor.AddModifier(armor);
        playerStatus.evasion.AddModifier(evasion);
        playerStatus.magicResistance.AddModifier(magicResistance);

        playerStatus.damage.AddModifier(damage);
        playerStatus.critChance.AddModifier(critChance);
        playerStatus.critPower.AddModifier(critPower);

        playerStatus.fireDamge.AddModifier(fireDamge);
        playerStatus.iceDamage.AddModifier(iceDamage);
        playerStatus.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();

        playerStatus.strength.RemoveModifier(strength);
        playerStatus.agility.RemoveModifier(agility);
        playerStatus.intelligence.RemoveModifier(intelligence);
        playerStatus.vitality.RemoveModifier(vitality);

        playerStatus.maxHealth.RemoveModifier(maxHealth);
        playerStatus.armor.RemoveModifier(armor);
        playerStatus.evasion.RemoveModifier(evasion);
        playerStatus.magicResistance.RemoveModifier(magicResistance);

        playerStatus.damage.RemoveModifier(damage);
        playerStatus.critChance.RemoveModifier(critChance);
        playerStatus.critPower.RemoveModifier(critPower);

        playerStatus.fireDamge.RemoveModifier(fireDamge);
        playerStatus.iceDamage.RemoveModifier(iceDamage);
        playerStatus.lightingDamage.RemoveModifier(lightingDamage);
    }
}
