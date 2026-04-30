using UnityEngine;

public class UITest : MonoBehaviour
{
    void Start()
    {
        ScoreUI.Instance.UpdateScore(999);
        ScoreUI.Instance.UpdateCombo(3.5f);
    }
}