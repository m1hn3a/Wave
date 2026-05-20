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
            return;

        if (!TokenSystem.Instance.SpendToken(cost))
            return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.upgradeSFX);

        playerShoot.bulletPierce++;

        // 🔥 Salvăm în UpgradeManager
        UpgradeManager.pierceLevel = playerShoot.bulletPierce;
        UpgradeManager.tokens = TokenSystem.Instance.tokens;
        UpgradeManager.SaveAll();
    }

    void UpdateUpgradeText()
    {
        if (playerShoot.bulletPierce >= maxPierceLevel)
        {
            upgradeText.text = "Bullet Piercing: MAX LEVEL";
            return;
        }

        upgradeText.text = "Increase Bullet Piercing: 1tk";
    }
}
