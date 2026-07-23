using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [Header("체력바가 표시할 유닛")]
    [SerializeField] private Unit targetUnit;

    [Header("UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    public void SetTarget(Unit newTarget)
    {
        targetUnit = newTarget;
        Refresh();
    }

    private void Refresh()
    {
        if (targetUnit == null || hpSlider == null)
        {
            return;
        }

        hpSlider.minValue = 0f;
        hpSlider.maxValue = targetUnit.maxHp;
        hpSlider.value = targetUnit.hp;

        if (hpText != null)
        {
            hpText.text =
                $"{Mathf.CeilToInt(targetUnit.hp)} / " +
                $"{Mathf.CeilToInt(targetUnit.maxHp)}";
        }
    }
}