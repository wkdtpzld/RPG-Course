using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    [SerializeField] private string statName;
    [SerializeField] private StatusType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    private void Start()
    {
        UpdateStatValue();
        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValue()
    {
        PlayerStatus status = PlayerManager.instance.player.GetComponent<PlayerStatus>();

        if (status != null)
        {
            statValueText.text = status.GetStatusValue(statType).GetValue().ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideStatTooltip();
    }
}
