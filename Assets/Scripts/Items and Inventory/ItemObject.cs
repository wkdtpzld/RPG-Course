using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 velocity;

    private void OnValidate()
    {
        SetUpVisuals();
    }

    private void SetUpVisuals()
    {
        if (itemData == null) return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "item object - " + itemData.itemName;
    }

    public void SetUpItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.linearVelocity = _velocity;

        SetUpVisuals();
    }

    public void PickUpItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
