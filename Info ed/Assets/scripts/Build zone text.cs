using UnityEngine;
using TMPro;

public class BuildZoneText : MonoBehaviour
{
    public TMP_Text buildText;

    public string normalText = "";
    public string pausedText = "‎ ‎ ‎ ‎ ‎ Can't repair core while paused";

    private void Start()
    {
        buildText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        buildText.text = SPAWNER.wavePaused ? pausedText : normalText;
        buildText.gameObject.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        buildText.text = SPAWNER.wavePaused ? pausedText : normalText;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        buildText.gameObject.SetActive(false);
    }
}
