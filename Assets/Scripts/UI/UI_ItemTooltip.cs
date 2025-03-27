using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontsize = 32;

    public void ShowToolTip(ItemData _itemData)
    {
        if (_itemData == null) return;
        itemNameText.text = _itemData.itemName;
        itemTypeText.text = _itemData.itemType.ToString();
        itemDescription.text = _itemData.GetDescription();

        if (itemNameText.text.Length > 12)
        {
            itemNameText.fontSize = itemNameText.fontSize * .7f;
        }
        else
        {
            itemNameText.fontSize = defaultFontsize;
        }

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontsize;
        gameObject.SetActive(false);
    }
}
