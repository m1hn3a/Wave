using UnityEngine;

public class BulletPiercingUpgradeStation : MonoBehaviour
{
    public PlayerShoot playerShoot;   // tragi Player-ul aici
    public int cost = 1;              // cost fix
    public int maxPierceLevel = 3;    // după 3 → infinite

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyPierceUpgrade();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered PIERCE upgrade zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void BuyPierceUpgrade()
    {
        if (!TokenSystem.Instance.SpendToken(cost))
        {
            Debug.Log("NU AI DESTUI TOKENI!");
            return;
        }

        // dacă nu e la max → crește pierce
        if (playerShoot.bulletPierce < maxPierceLevel)
        {
            playerShoot.bulletPierce++;
            Debug.Log("Pierce upgraded → " + playerShoot.bulletPierce);
        }
        else
        {
            // infinite pierce
            playerShoot.bulletPierce = 9999;
            Debug.Log("MAX PIERCE → Infinite!");
        }
    }
}
