using UnityEngine;


[CreateAssetMenu(fileName = "new Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}
