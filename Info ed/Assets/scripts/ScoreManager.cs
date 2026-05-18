using UnityEngine;
using TMPro;

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

    // 🔥 NOU
    private bool waitingForFirstKill = false;
    private float waveStartTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // 🔥 PAUZĂ LA ÎNCEPUT DE WAVE
        if (waitingForFirstKill)
        {
            waveStartTimer -= Time.deltaTime;

            if (waveStartTimer <= 0f)
            {
                comboPaused = false;
                waitingForFirstKill = false;
            }

            return; // nu rulăm restul logicii cât timp e pauză
        }

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

    // 🔥 APELI ASTA CÂND ÎNCEPE WAVE-UL
    public void PauseScoreAtWaveStart()
    {
        comboPaused = true;
        waitingForFirstKill = true;
        waveStartTimer = 3f; // 3 secunde pauză
    }

    public void AddKill(int basePoints)
    {
        // 🔥 dacă moare primul inamic, scoatem pauza instant
        if (waitingForFirstKill)
        {
            waitingForFirstKill = false;
            comboPaused = false;
        }

        comboMultiplier += comboIncrease;
        comboTimer = comboDuration;

        ScoreUI.Instance.UpdateComboSlider(1f);

        int pointsToAdd = Mathf.RoundToInt(basePoints * comboMultiplier);
        score += pointsToAdd;

        ScoreUI.Instance.UpdateScore(score);
        ScoreUI.Instance.UpdateCombo(comboMultiplier);
    }
}
