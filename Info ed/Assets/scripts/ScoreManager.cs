using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public float comboMultiplier = 1f;
    public float comboTimer = 0f;

    [Header("Combo Settings")]
    public float comboDuration = 1.2f;
    public float comboIncrease = 0.2f;

    [HideInInspector]
    public bool comboPaused = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (comboPaused)
            return;

        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;

            float normalized = comboTimer / comboDuration;
            ScoreUI.Instance.UpdateComboSlider(normalized);

            if (comboTimer <= 0)
            {
                comboMultiplier = 1f;
                ScoreUI.Instance.HideCombo();
            }
        }
    }

    public void AddKill(int basePoints)
    {
        comboMultiplier += comboIncrease;
        comboTimer = comboDuration;

        ScoreUI.Instance.UpdateComboSlider(1f);

        int pointsToAdd = Mathf.RoundToInt(basePoints * comboMultiplier);
        score += pointsToAdd;

        ScoreUI.Instance.UpdateScore(score);
        ScoreUI.Instance.UpdateCombo(comboMultiplier);
    }
}