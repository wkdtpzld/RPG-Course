using UnityEngine;
using UnityEngine.EventSystems;


public class UI_CraftSlot : UI_ItemSlot
{
   private void OnEnable()
   {
      UpdateSlot(item);
   }

   public override void OnPointerDown(PointerEventData eventData)
   {
      ItemData_Equipment itemData = item.data as ItemData_Equipment;

      Inventory.instance.CanCraft(itemData, itemData.craftingMaterials);
   }
}
