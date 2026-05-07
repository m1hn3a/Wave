using UnityEngine;

public class MovementSpeedUpgradeStation : MonoBehaviour
{
    public movement playerMovement;
    public int cost = 1;
    public float speedIncreaseFlat = 0.75f;
    
 // 🔥 creștere lentă
    public int maxLevel = 5;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
            BuySpeedUpgrade();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    void BuySpeedUpgrade()
    {
        if (playerMovement.speedLevel >= maxLevel)
        {
            Debug.Log("Movement speed MAX LEVEL!");
            return;
        }

        if (!TokenSystem.Instance.SpendToken(cost))
        {
            Debug.Log("NU AI DESTUI TOKENI!");
            return;
        }

        playerMovement.IncreaseSpeed(speedIncreaseFlat);
        playerMovement.speedLevel++;
    }
}
