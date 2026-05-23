using UnityEngine;
using TMPro;

public class Core : MonoBehaviour
{
    [Header("Enemy Damage")]
    public int damagePerHit = 10;

    [Header("Core Health")]
    public float maxHP = 200;
    public float currentHP;

    [Header("Heal Settings")]
    public float healPerSecond = 5f;
    public float maxPauseHealTime = 5f;

    [Header("UI")]
    public TMP_Text buildText;

    private bool playerInside = false;

    public bool playerInsideRepair
    {
        get => playerInside;
        set => playerInside = value;
    }

    private float pauseHealTimer = 0f;

    private movement playerMove;
    private PlayerShoot playerShoot;

    private float slowMultiplier = 0.75f;
    private bool speedReduced = false;

    //   FIX: prevenim apelarea repetată a OnCoreDestroyed()
    private bool coreDestroyed = false;

    void Start()
    {
        currentHP = maxHP;

        if (buildText != null)
            buildText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentHP <= 0)
        {
            OnCoreDestroyed();
            return;
        }

        if (!playerInside)
            return;

        bool paused = SPAWNER.wavePaused;
        bool holdingE = Input.GetKey(KeyCode.E);

        if (!paused)
        {
            pauseHealTimer = 0f;

            if (holdingE)
                HealCore();

            return;
        }

        if (holdingE && pauseHealTimer < maxPauseHealTime)
        {
            float healTime = Mathf.Min(Time.deltaTime, maxPauseHealTime - pauseHealTimer);

            currentHP += healPerSecond * healTime;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);

            pauseHealTimer += healTime;
        }
    }

    private void HealCore()
    {
        currentHP += healPerSecond * Time.deltaTime;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    public void RepairCore()
    {
        HealCore();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        playerMove = other.GetComponent<movement>();
        playerShoot = other.GetComponent<PlayerShoot>();

        if (playerMove != null && !speedReduced)
        {
            playerMove.moveSpeed *= slowMultiplier;
            speedReduced = true;
        }

        if (playerShoot != null)
            playerShoot.canShoot = false;

        UpdateText();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        UpdateText();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (playerMove != null && speedReduced)
        {
            playerMove.moveSpeed /= slowMultiplier;
            speedReduced = false;
        }

        if (playerShoot != null)
            playerShoot.canShoot = true;

        if (buildText != null)
            buildText.gameObject.SetActive(false);
    }

    private void UpdateText()
    {
        if (buildText == null)
            return;

        if (SPAWNER.wavePaused)
            buildText.text = "Hold E to heal (max 5s)";
        else
            buildText.text = "Hold E to heal. Shooting is disabled";

        buildText.gameObject.SetActive(true);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(Mathf.RoundToInt(10f * Time.deltaTime));
        }
    }

    void OnCoreDestroyed()
    {
        //   FIX: rulează o singură dată
        if (coreDestroyed) return;
        coreDestroyed = true;

        // 🔊 Sunet de explozie reactor
        AudioManager.Instance.PlaySFX(AudioManager.Instance.reactorDeathSFX);

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
