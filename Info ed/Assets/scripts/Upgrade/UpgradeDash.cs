using UnityEngine;
using TMPro;

public class UpgradeDashPower : MonoBehaviour
{
    [Header("References")]
    public Dash dashScript;                   // tragi Player-ul aici
    public TextMeshProUGUI upgradeText;       // TMP-ul din canvasul playerului

    [Header("Settings")]
    public int cost = 1;
    public float dashIncreaseFlat = 3.5f;     // creștere per nivel
    public int maxLevel = 5;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuyDashPowerUpgrade();
            UpdateUpgradeText(); // actualizăm textul după upgrade
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            UpdateUpgradeText();               // afișăm textul când intră
            upgradeText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            upgradeText.gameObject.SetActive(false); // ascundem textul
        }
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

   void UpdateUpgradeText()
{
    // MAX LEVEL
    if (dashScript.dashLevel >= maxLevel)
    {
        upgradeText.text = "Dash Power: MAX LEVEL";
        return;
    }

    // NIVEL NORMAL
    upgradeText.text =
        "Increase Dash level : " + cost + "tk";
}

}
