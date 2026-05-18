using UnityEngine;
using TMPro;

public class WavePauseBlink : MonoBehaviour
{
    public TMP_Text pauseText;
    public float blinkSpeed = 1.5f;

    void Update()
    {
        if (!SPAWNER.wavePaused)
        {
            pauseText.alpha = 1f;
            return;
        }

        float a = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        pauseText.alpha = a;
    }
}
