using UnityEngine;

public class HealthUpgradeStation : MonoBehaviour
{
    public PlayerHealth playerHealth;   // tragi Player-ul aici
    public int healCost = 1;            // mereu 1 token

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyHeal();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered HEAL zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void BuyHeal()
    {
        // verificăm tokenii
        if (!TokenSystem.Instance.SpendToken(healCost))
        {
            Debug.Log("NU AI DESTUI TOKENI!");
            return;
        }

        // umple viața
        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.UpdateHealthUI();

        Debug.Log("HP FULL → Ai fost vindecat complet!");
    }
}
