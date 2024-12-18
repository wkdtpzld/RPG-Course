using UnityEngine;

public class ItemObject_trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<CharacterStatus>().isDead) return;
            myItemObject.PickUpItem();
        }
    }
}
