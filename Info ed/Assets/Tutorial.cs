using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialSlide
    {
        public string text;
        public float duration = 3f;
    }

    [Header("Slides")]
    public TutorialSlide[] slides;

    [Header("UI")]
    public TMP_Text tutorialText;

    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
{
    SPAWNER.tutorialMode = true;

    SPAWNER.forcePaused = true;
    SPAWNER.wavePaused = true;
    SPAWNER.waveFinished = false;

    ShowSlide(0);
}


    void Update()
    {
        // 🔥 ENTER → sari tutorialul instant
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EndTutorial();
            return;
        }

        if (slides.Length == 0)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
            NextSlide();
    }

    void ShowSlide(int index)
    {
        currentIndex = index;
        tutorialText.text = slides[index].text;
        timer = slides[index].duration;
    }

    void NextSlide()
    {
        if (currentIndex + 1 >= slides.Length)
        {
            EndTutorial();
            return;
        }

        ShowSlide(currentIndex + 1);
    }

   void EndTutorial()
{
    SPAWNER.tutorialMode = false;

    SPAWNER.forcePaused = false;
    SPAWNER.wavePaused = false;
    SPAWNER.waveFinished = true;

    SceneManager.LoadScene("SampleScene");
}

}
