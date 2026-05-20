using UnityEngine;
using TMPro;

public class ShootingUpgradeStation : MonoBehaviour
{
    public PlayerShoot playerShoot;
    public TextMeshProUGUI upgradeText;

    private bool playerInside = false;

    void Start()
    {
        upgradeText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyUpgrade();
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

    void BuyUpgrade()
    {
        int cost = 1;

        if (!TokenSystem.Instance.SpendToken(cost))
            return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.upgradeSFX);

        switch (playerShoot.fireMode)
        {
            case FireMode.SingleReload:
                playerShoot.fireMode = FireMode.SemiAuto;
                break;

            case FireMode.SemiAuto:
                playerShoot.fireMode = FireMode.FullAuto;
                break;

            case FireMode.FullAuto:
                playerShoot.fireMode = FireMode.TripleShot;
                break;

            case FireMode.TripleShot:
                return;
        }

        playerShoot.SetAmmoForMode();

        
        UpgradeManager.fireMode = playerShoot.fireMode;
        UpgradeManager.tokens = TokenSystem.Instance.tokens;
        UpgradeManager.SaveAll();
    }

    void UpdateUpgradeText()
    {
        switch (playerShoot.fireMode)
        {
            case FireMode.SingleReload:
                upgradeText.text = "Unlock Semi-Auto: 1tk";
                break;

            case FireMode.SemiAuto:
                upgradeText.text = "Unlock Full Auto: 1tk";
                break;

            case FireMode.FullAuto:
                upgradeText.text = "Unlock Triple Shot: 1tk";
                break;

            case FireMode.TripleShot:
                upgradeText.text = "Weapon Upgrade: MAX LEVEL";
                break;
        }
    }
}
