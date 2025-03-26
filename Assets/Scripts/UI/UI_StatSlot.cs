using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] private StatusType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

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
    }

    public void UpdateStatValue()
    {
        PlayerStatus status = PlayerManager.instance.player.GetComponent<PlayerStatus>();

        if (status != null)
        {
            statValueText.text = status.GetStatusValue(statType).GetValue().ToString();
        }
    }
}
