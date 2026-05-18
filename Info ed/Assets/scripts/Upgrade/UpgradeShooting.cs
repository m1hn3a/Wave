using UnityEngine;
using TMPro;

public class ShootingUpgradeStation : MonoBehaviour
{
    public PlayerShoot playerShoot;
    public TextMeshProUGUI upgradeText;

    private bool playerInside = false;

    void Start()
    {
        if (upgradeText != null)
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

            if (upgradeText != null)
                upgradeText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (upgradeText != null)
                upgradeText.gameObject.SetActive(false);
        }
    }

    void BuyUpgrade()
    {
        int cost = GetUpgradeCost();

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
                return;
        }

        // 🔥 IMPORTANT — actualizează ammo pentru noul mod
        playerShoot.SetAmmoForMode();
    }

    int GetUpgradeCost()
    {
        switch (playerShoot.fireMode)
        {
            case FireMode.SingleReload: return 1;
            case FireMode.SemiAuto:     return 1;
            case FireMode.FullAuto:     return 1;
            case FireMode.TripleShot:   return 0;
        }
        return 0;
    }

    // 🔥 TEXTELE DE UPGRADE
    void UpdateUpgradeText()
    {
        if (upgradeText == null) return;

        switch (playerShoot.fireMode)
        {
            case FireMode.SingleReload:
                upgradeText.text =
                    "Unlocks Semi-Auto fire mode \n" +
                    "Cost: 1tk";
                break;

            case FireMode.SemiAuto:
                upgradeText.text =
                    "Unlocks Full Auto \n" +
                    "Cost: 1tk";
                break;

            case FireMode.FullAuto:
                upgradeText.text =
                    "Unlocks Triple Shot \n" +
                    "Cost: 1tk";
                break;

            case FireMode.TripleShot:
                upgradeText.text =
                    "Weapon Upgrade: MAX LEVEL";
                break;
        }
    }
}
