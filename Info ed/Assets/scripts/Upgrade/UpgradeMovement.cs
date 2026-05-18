using UnityEngine;
using TMPro;

public class MovementSpeedUpgradeStation : MonoBehaviour
{
    [Header("References")]
    public movement playerMovement;            // tragi Player-ul aici
    public TextMeshProUGUI upgradeText;        // TMP-ul din canvasul playerului

    [Header("Settings")]
    public int cost = 1;
    public float speedIncreaseFlat = 0.75f;
    public int maxLevel = 5;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            BuySpeedUpgrade();
            UpdateUpgradeText(); // actualizăm textul după cumpărare
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            UpdateUpgradeText(); // afișăm textul când intră
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

    void UpdateUpgradeText()
    {
        if (playerMovement.speedLevel >= maxLevel)
        {
            upgradeText.text = "Movement Speed: MAX LEVEL";
            return;
        }

 upgradeText.text =
    "Increase Movement Speed : 1tk\n" +
    "Current Level: " + playerMovement.speedLevel + " / " + maxLevel;
    }}