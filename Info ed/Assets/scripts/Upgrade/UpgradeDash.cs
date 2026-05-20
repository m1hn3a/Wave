using UnityEngine;
using TMPro;

public class UpgradeDashPower : MonoBehaviour
{
    [Header("References")]
    public Dash dashScript;
    public TextMeshProUGUI upgradeText;

    [Header("Settings")]
    public int cost = 1;
    public float dashIncreaseFlat = 3.5f;
    public int maxLevel = 5;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyDashPowerUpgrade();
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

    void BuyDashPowerUpgrade()
    {
        if (dashScript.dashLevel >= maxLevel)
            return;

        if (!TokenSystem.Instance.SpendToken(cost))
            return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.upgradeSFX);

        dashScript.UpgradeDashPower(dashIncreaseFlat);
        dashScript.dashLevel++;

        // 🔥 Salvăm în UpgradeManager
        UpgradeManager.dashLevel = dashScript.dashLevel;
        UpgradeManager.tokens = TokenSystem.Instance.tokens;
        UpgradeManager.SaveAll();
    }

    void UpdateUpgradeText()
    {
        if (dashScript.dashLevel >= maxLevel)
        {
            upgradeText.text = "Dash Power: MAX LEVEL";
            return;
        }

        upgradeText.text = "Increase Dash Power: 1tk";
    }
}
