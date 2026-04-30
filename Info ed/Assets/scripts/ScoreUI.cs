using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    public Slider comboSlider;
    public ComboTextAnimator animator;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateCombo(float multiplier)
    {
        comboText.gameObject.SetActive(true);
        comboText.text = "COMBO x" + multiplier.ToString("0.0");

        if (animator != null)
            animator.PlayPop(multiplier);
    }

    public void UpdateComboSlider(float value)
    {
        if (comboSlider != null)
            comboSlider.value = value;
    }

    public void HideCombo()
    {
        comboText.gameObject.SetActive(false);

        if (comboSlider != null)
            comboSlider.value = 0;
    }
}