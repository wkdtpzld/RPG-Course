using System.Text;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment,
}


[CreateAssetMenu(fileName = "new Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }
}
