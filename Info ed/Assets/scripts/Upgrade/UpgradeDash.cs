using UnityEngine;

public class UpgradeDashPower : MonoBehaviour
{
    public Dash dashScript;
    public int cost = 1;
    public float dashIncreaseFlat = 3.5f; // 🔥 calculat pentru max 22
    public int maxLevel = 5;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
            BuyDashPowerUpgrade();
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

    void BuyDashPowerUpgrade()
    {
        if (dashScript.dashLevel >= maxLevel)
        {
            Debug.Log("Dash MAX LEVEL!");
            return;
        }

        if (!TokenSystem.Instance.SpendToken(cost))
        {
            Debug.Log("NU AI DESTUI TOKENI!");
            return;
        }

        dashScript.UpgradeDashPower(dashIncreaseFlat);
        dashScript.dashLevel++;
    }
}
