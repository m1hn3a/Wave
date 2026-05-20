using UnityEngine;
using TMPro;

public class HealthUpgradeStation : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public TextMeshProUGUI upgradeText;

    [Header("Settings")]
    public int healCost = 1;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyHeal();
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

    void BuyHeal()
    {
        if (playerHealth.currentHealth >= playerHealth.maxHealth)
            return;

        if (!TokenSystem.Instance.SpendToken(healCost))
            return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.upgradeSFX);

        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.UpdateHealthUI();

        // 🔥 Salvăm tokenii
        UpgradeManager.tokens = TokenSystem.Instance.tokens;
        UpgradeManager.SaveAll();
    }

    void UpdateUpgradeText()
    {
        upgradeText.text = "Fully heals the player: 1tk";
    }
}
