using UnityEngine;
using TMPro;

public class MovementSpeedUpgradeStation : MonoBehaviour
{
    [Header("References")]
    public movement playerMovement;
    public TextMeshProUGUI upgradeText;

    [Header("Settings")]
    public int cost = 1;
    public float speedIncreaseFlat = 0.5f;
    public int maxLevel = 4;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuySpeedUpgrade();
            UpdateUpgradeText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            UpdateUpgradeText();
            upgradeText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            upgradeText.gameObject.SetActive(false);
        }
    }

    void BuySpeedUpgrade()
    {
        if (playerMovement.speedLevel >= maxLevel)
            return;

        if (!TokenSystem.Instance.SpendToken(cost))
            return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.upgradeSFX);

        playerMovement.IncreaseSpeed(speedIncreaseFlat);

        //  Salvăm în UpgradeManager
        UpgradeManager.speedLevel = playerMovement.speedLevel;
        UpgradeManager.tokens = TokenSystem.Instance.tokens;
        UpgradeManager.SaveAll();
    }

    void UpdateUpgradeText()
    {
        if (playerMovement.speedLevel >= maxLevel)
        {
            upgradeText.text = "Movement Speed: MAX LEVEL";
            return;
        }

        upgradeText.text =
            "Increase Movement Speed: 1tk\n" +
            "Current Level: " + playerMovement.speedLevel + "/" + maxLevel;
    }
}
