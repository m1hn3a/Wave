using UnityEngine;
using TMPro;

public class Core : MonoBehaviour
{
    [Header("Enemy Damage")]
public int damagePerHit = 10;

    [Header("Core Health")]
    public float maxHP = 200;
    public float currentHP;

    [Header("Damage & Repair Settings")]
    public float repairPerSecond = 2;
    public float enemyDamagePerSecond = 10f;

    [Header("UI")]
    public TMP_Text buildText;

    public bool playerInsideRepair = false;

    private movement playerMove;
    private PlayerShoot playerShoot;

    private float originalSpeed;
    private bool speedReduced = false;

    void Start()
    {
        currentHP = maxHP;

        if (buildText != null)
            buildText.gameObject.SetActive(false);
    }

    void Update()
{
    // 1. Reactorul poate muri ORICÂND, indiferent de pauză
    if (currentHP <= 0)
    {
        OnCoreDestroyed();
        return;
    }

    // 2. Dacă wave-ul e pe pauză → blocăm DOAR repair-ul
    if (SPAWNER.wavePaused)
        return;

    // 3. Dacă playerul e în zona de repair → reparăm
    if (playerInsideRepair)
        RepairCore();
}


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInsideRepair = true;

        playerMove = other.GetComponent<movement>();
        playerShoot = other.GetComponent<PlayerShoot>();

        if (playerMove != null && !speedReduced)
        {
            originalSpeed = playerMove.moveSpeed;
            playerMove.moveSpeed *= 0.75f;
            speedReduced = true;
        }

        if (playerShoot != null)
            playerShoot.canShoot = false;

        if (buildText != null)
        {
            buildText.text = SPAWNER.wavePaused ?
                "Can't repair core while paused" :
                "Press E to repair core";

            buildText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (buildText != null)
        {
            buildText.text = SPAWNER.wavePaused ?
                "Can't repair core while paused" :
                "Press E to repair core";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInsideRepair = false;

        if (playerMove != null && speedReduced)
        {
            playerMove.moveSpeed = originalSpeed;
            speedReduced = false;
        }

        if (playerShoot != null)
            playerShoot.canShoot = true;

        if (buildText != null)
            buildText.gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(Mathf.RoundToInt(enemyDamagePerSecond * Time.deltaTime));
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    public void RepairCore()
    {
        currentHP += repairPerSecond * Time.deltaTime;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

   void OnCoreDestroyed()
{
    var death = FindAnyObjectByType<DeathScreen>();
    if (death != null)
    {
        int finalScore = ScoreManager.Instance.score;
        int finalWave = FindAnyObjectByType<SPAWNER>().currentWave;

        death.ShowDeathScreen("REACTOR", finalScore, finalWave);
    }

    SPAWNER.wavePaused = true;

    var player = FindAnyObjectByType<movement>();
    if (player != null)
        player.canMove = false;
}

}
