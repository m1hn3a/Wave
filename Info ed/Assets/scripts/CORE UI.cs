using UnityEngine;
using UnityEngine.UI;

public class ReactorHPUI : MonoBehaviour
{
    public Core core;        // tragi reactorul aici
    public Slider hpSlider;  // tragi slider-ul aici

    void Start()
    {
        hpSlider.maxValue = core.maxHP;
        hpSlider.value = core.currentHP;
    }

    void Update()
    {
        hpSlider.value = core.currentHP;
    }
}
