using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStatus myStatus;
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStatus = GetComponentInParent<CharacterStatus>();

        entity.onFliped += FlipUI;
        myStatus.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFliped -= FlipUI;
        myStatus.onHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStatus.GetMaxHealthValue();
        slider.value = myStatus.currentHealth;
    }
}
