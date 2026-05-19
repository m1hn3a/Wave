using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public string gameplaySceneName = "SampleScene";
    public static bool loadRequested = false;

    public void NewGame()
    {
        loadRequested = false;

        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
            File.Delete(path);

        SceneManager.LoadScene(gameplaySceneName);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/save.json";

        if (!File.Exists(path))
        {
            Debug.Log("No save found!");
            return;
        }

        loadRequested = true;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
