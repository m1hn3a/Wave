using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    [Header("Panel")]
    public GameObject deathPanel;

    [Header("Texts")]
    public TMP_Text reactorDeathText;
    public TMP_Text playerDeathText;
    public TMP_Text scoreText;
    public TMP_Text waveText;

    private bool shown = false;

    void Start()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    public void ShowDeathScreen(string cause, int score, int wave)
    {
        if (shown)
            return;

        shown = true;

        deathPanel.SetActive(true);

        reactorDeathText.gameObject.SetActive(false);
        playerDeathText.gameObject.SetActive(false);

        if (cause == "REACTOR")
            reactorDeathText.gameObject.SetActive(true);

        if (cause == "PLAYER")
            playerDeathText.gameObject.SetActive(true);

        scoreText.text = "Score: " + score;
        waveText.text = "Wave: " + wave;

        Time.timeScale = 0f;
    }

    // 🔁 Restart jocul
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 🏠 Mergi la meniul principal
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
