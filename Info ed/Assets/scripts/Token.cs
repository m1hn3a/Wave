using UnityEngine;
using TMPro;

public class TokenSystem : MonoBehaviour
{
    public static TokenSystem Instance;

    public int tokens = 0;
    public TMP_Text tokenText;
    public SPAWNER spawner;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // UI-ul cu tokeni apare doar în pauză
        bool inPause = spawner.waveFinished && !spawner.waveActive;

        tokenText.gameObject.SetActive(inPause);

        if (inPause)
            tokenText.text = "Tokens: " + tokens;
    }

    public void AddToken()
    {
        tokens++;
        Debug.Log("TOKEN +1 → Total: " + tokens);
    }

    // 🔥 Funcția necesară pentru upgrade-uri
    public bool SpendToken(int amount)
    {
        if (tokens >= amount)
        {
            tokens -= amount;
            Debug.Log("TOKEN -"+ amount +" → Total: " + tokens);
            return true;
        }

        Debug.Log("NU AI DESTUI TOKENI!");
        return false;
    }
}
