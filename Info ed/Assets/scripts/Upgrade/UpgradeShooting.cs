using UnityEngine;

public class ShootingUpgradeStation : MonoBehaviour
{
    public PlayerShoot playerShoot;   

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyUpgrade();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered shooting upgrade zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void BuyUpgrade()
    {
        int cost = GetUpgradeCost();

        Debug.Log("Upgrade cost: " + cost);

        if (!TokenSystem.Instance.SpendToken(cost))
        {
            Debug.Log("NU AI DESTUI TOKENI!");
            return;
        }

        switch (playerShoot.fireMode)
        {
            case FireMode.SingleReload:
                playerShoot.fireMode = FireMode.SemiAuto;
                Debug.Log("UPGRADE 1 → SEMI AUTO");
                break;

            case FireMode.SemiAuto:
                playerShoot.fireMode = FireMode.FullAuto;
                Debug.Log("UPGRADE 2 → FULL AUTO");
                break;

            case FireMode.FullAuto:
                playerShoot.fireMode = FireMode.TripleShot;
                Debug.Log("UPGRADE 3 → TRIPLE SHOT");
                break;

            case FireMode.TripleShot:
                Debug.Log("MAX UPGRADE REACHED");
                break;
        }
    }

    int GetUpgradeCost()
    {
        switch (playerShoot.fireMode)
        {
            case FireMode.SingleReload: return 1; // Semi-auto
            case FireMode.SemiAuto:     return 1; // Full-auto
            case FireMode.FullAuto:     return 1; // Triple shot
            case FireMode.TripleShot:   return 0; // Max
        }

        return 0;
    }
}
