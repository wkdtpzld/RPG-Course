using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        Inventory.instance.Unequipment(item.data as ItemData_Equipment);

        ui.itemTooltip.HideToolTip();

        CleanUpSlot();
    }
}
