using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player`s drop")]
    [SerializeField] private float changeToLooseItems;

    public override void GenerateDrop()
    {
        // list of Equipment
        Inventory inventory = Inventory.instance;
        List<InventoryItem> equipmentList = inventory.GetEquipmentList();
        List<InventoryItem> stashList = inventory.GetStashList();
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> itemsToDrop = new List<InventoryItem>();


        // foreach item we gonna check if should loose items
        foreach (InventoryItem item in equipmentList)
        {
            if (Random.Range(0, 100) <= changeToLooseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        foreach (InventoryItem item in stashList)
        {
            if (Random.Range(0, 100) <= changeToLooseItems)
            {
                DropItem(item.data);
                itemsToDrop.Add(item);
            }
        }

        foreach (InventoryItem item in itemsToUnequip)
        {
            inventory.Unequipment(item.data as ItemData_Equipment);
        }

        foreach (InventoryItem item in itemsToDrop)
        {
            inventory.RemoveItem(item.data);
        }
    }
}
