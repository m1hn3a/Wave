using UnityEngine;
using TMPro;
using System.Collections;

public class ComboTextAnimator : MonoBehaviour
{
    public TextMeshProUGUI comboText;

    RectTransform rect;
    Vector2 basePos;

    float popTime = 0.25f;
    float timer = 0f;

    float shakeStrength = 0f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        basePos = rect.anchoredPosition;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            float t = 1f - (timer / popTime);

            // POP SCALE
            float scale = Mathf.Lerp(1.8f, 1f, t);
            rect.localScale = new Vector3(scale, scale, 1);

            // BOUNCE UP
            float moveUp = Mathf.Lerp(0, 40, t);

            // SHAKE
            float shakeX = Random.Range(-shakeStrength, shakeStrength);
            float shakeY = Random.Range(-shakeStrength, shakeStrength);

            rect.anchoredPosition = basePos + new Vector2(shakeX, moveUp + shakeY);
        }
        else
        {
            rect.anchoredPosition = basePos;
            rect.localScale = Vector3.one;
        }
    }

    public void PlayPop(float multiplier)
    {
        StartCoroutine(DelayedPop(multiplier));
    }

    private IEnumerator DelayedPop(float multiplier)
    {
        yield return null; // wait 1 frame so UI is active

        timer = popTime;

        // COLOR FLASH
        if (multiplier < 2f)
            comboText.color = Color.yellow;
        else if (multiplier < 3f)
            comboText.color = new Color(1f, 0.6f, 0f); // orange
        else
            comboText.color = Color.red;

        // SHAKE AT HIGH COMBO
        shakeStrength = (multiplier >= 3f) ? 4f : 0f;
    }
}