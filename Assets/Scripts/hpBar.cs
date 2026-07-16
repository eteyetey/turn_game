using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class hpBar : MonoBehaviour
{
    public Slider slider;
    public Unit target;
    public TMP_Text text;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        target = GetComponentInParent<Unit>();
        text = GetComponentInChildren<TMP_Text>();
    }
    private void Start()
    {
        slider.maxValue = target.maxhp;
        slider.value = target.hp;
        text.SetText($"{target.hp:0}/{target.maxhp:0}");
    }
    private void Update()
    {
        slider.value = target.hp;
        text.SetText($"{target.hp:0}/{target.maxhp:0}");
    }

}
