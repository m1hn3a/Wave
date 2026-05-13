using UnityEngine;
using TMPro;

public class ShootingUpgradeStation : MonoBehaviour
{
    public PlayerShoot playerShoot;
    public int upgradeCost = 1;

    [Header("UI Upgrade Text")]
    public TextMeshProUGUI upgradeText;   // 🔥 tragi TMP-ul aici
    public string[] upgradeDescriptions;  // 🔥 text pentru fiecare upgrade

    private bool playerInside = false;

    void Start()
    {
        if (upgradeText != null)
            upgradeText.gameObject.SetActive(false); // ascuns la început
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyUpgrade();
            UpdateUpgradeText(); // 🔥 actualizează textul după upgrade
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            UpdateUpgradeText(); // 🔥 afișează textul corect
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
        if (!TokenSystem.Instance.SpendToken(upgradeCost))
        {
            Debug.Log("NU AI DESTUI TOKENI!");
            return;
        }

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
                Debug.Log("MAX UPGRADE");
                break;
        }
    }

    // 🔥 AICI SE SCHIMBĂ TEXTUL ÎN FUNCȚIE DE UPGRADE
    void UpdateUpgradeText()
    {
        if (upgradeText == null) return;

        int index = 0;

        switch (playerShoot.fireMode)
        {
            case FireMode.SingleReload: index = 0; break;
            case FireMode.SemiAuto:     index = 1; break;
            case FireMode.FullAuto:     index = 2; break;
            case FireMode.TripleShot:   index = 3; break;
        }

        if (index < upgradeDescriptions.Length)
            upgradeText.text = upgradeDescriptions[index];
        else
            upgradeText.text = "MAX UPGRADE";
    }
}
