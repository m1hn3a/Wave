using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highscoreText;

    void Start()
    {
        int hs = PlayerPrefs.GetInt("highscore", 0);
        highscoreText.text = "Highscore: " + hs;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
