using UnityEngine;
using TMPro;

public class BulletPiercingUpgradeStation : MonoBehaviour
{
    [Header("References")]
    public PlayerShoot playerShoot;           
    public TextMeshProUGUI upgradeText;       

    [Header("Settings")]
    public int cost = 1;                      
    public int maxPierceLevel = 3;            

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyPierceUpgrade();
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

    void BuyPierceUpgrade()
    {
        if (playerShoot.bulletPierce >= maxPierceLevel)
        {
            Debug.Log("Piercing MAX LEVEL");
            return;
        }

        if (!TokenSystem.Instance.SpendToken(cost))
        {
            Debug.Log("NU AI DESTUI TOKENI!");
            return;
        }

        playerShoot.bulletPierce++;
        Debug.Log("Pierce upgraded → " + playerShoot.bulletPierce);
    }

    void UpdateUpgradeText()
    {
        // MAX LEVEL
        if (playerShoot.bulletPierce >= maxPierceLevel)
        {
            upgradeText.text =
                "Bullet Piercing: MAX LEVEL\n" +
                "Bullets pierce up to 3 enemies";
            return;
        }

        // NIVEL NORMAL
        switch (playerShoot.bulletPierce)
        {
            case 0:
                upgradeText.text =
                    "Unlocks Bullet Piercing (pierce 1 enemy)\n" +
                    "Cost: 1tk";
                break;

            case 1:
                upgradeText.text =
                    "Increase Piercing (pierce 2 enemies)\n" +
                    "Cost: 1tk";
                break;

            case 2:
                upgradeText.text =
                    "Increase Piercing (pierce 3 enemies)\n" +
                    "Cost: 1tk";
                break;
        }
    }
}
