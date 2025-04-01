using System.Collections.Generic;
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

    [Header("Unique effect")]
    public ItemEffect[] itemEffects;
    public float itemCooldown;
    [TextArea]
    public string itemEffectDescription;

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

    [Header("Craft Requirements")]
    public List<InventoryItem> craftingMaterials;

    private int minDescriptionLength;

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

    public void Effect(Transform _enemyPosition)
    {
        foreach (ItemEffect effect in itemEffects)
        {
            effect.ExecuteEffect(_enemyPosition);
        }
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        minDescriptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(maxHealth, "Max Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Resistance");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit Chance");
        AddItemDescription(critPower, "Crit Power");

        AddItemDescription(fireDamge, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightingDamage, "Lightning Damage");

        if (minDescriptionLength < 5)
        {
            for (int i = 0; i < 5 - minDescriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        if (itemEffectDescription.Length > 0)
        {
            sb.AppendLine();
            sb.Append(itemEffectDescription);
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (_value > 0)
            {
                sb.Append("+ " + _value + ": " + _name);
            }

            minDescriptionLength++;
        }
    }
}
