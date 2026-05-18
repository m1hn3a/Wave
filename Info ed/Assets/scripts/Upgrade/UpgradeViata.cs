using UnityEngine;
using TMPro;

public class HealthUpgradeStation : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;          // tragi Player-ul aici
    public TextMeshProUGUI upgradeText;        // TMP-ul din canvasul playerului

    [Header("Settings")]
    public int healCost = 1;                   // mereu 1 token

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyHeal();
            UpdateUpgradeText(); // textul rămâne același, dar îl chemăm pentru consistență
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            UpdateUpgradeText();               // afișăm textul când intră
            upgradeText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            upgradeText.gameObject.SetActive(false); // ascundem textul
        }
    }

    void BuyHeal()
    {
        // 🔥 NU POATE CUMPĂRA DACĂ VIAȚA E FULL
        if (playerHealth.currentHealth >= playerHealth.maxHealth)
        {
            Debug.Log("HP FULL → nu poți cumpăra heal.");
            return;
        }

        if (!TokenSystem.Instance.SpendToken(healCost))
        {
            Debug.Log("NU AI DESTUI TOKENI!");
            return;
        }

        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.UpdateHealthUI();

        Debug.Log("HP FULLY HEALED!");
    }

    void UpdateUpgradeText()
    {
        upgradeText.text =
            "Fully heals the player: 1tk";
    }
}
