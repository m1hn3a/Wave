using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthSlider;
    public bool invincible = false;

    [Header("Damage Flash")]
    public SpriteRenderer playerSprite;
    public float flashDuration = 0.1f;

    private bool isFlashing = false;
    private float flashTimer = 0f;

    [Header("Damage SFX")]
    public AudioSource audioSource;
    public AudioClip damageSFX;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Update()
    {
        if (isFlashing)
        {
            flashTimer -= Time.deltaTime;

            if (flashTimer <= 0f)
            {
                isFlashing = false;
                playerSprite.color = Color.white;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (invincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        FlashRed();
        PlayDamageSFX();
        UpdateHealthUI();

        if (currentHealth <= 0)
            Die();
    }

    void FlashRed()
    {
        if (playerSprite == null) return;

        if (!isFlashing)
        {
            isFlashing = true;
            playerSprite.color = new Color(1f, 0.3f, 0.3f);
        }

        flashTimer = flashDuration;
    }

    void PlayDamageSFX()
    {
        if (audioSource != null && damageSFX != null)
        {
            audioSource.PlayOneShot(damageSFX, sfxVolume);
        }
    }

    public void HealToFull()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        Debug.Log("HP FULL → viața a fost umplută complet!");
    }

    public void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    public void Die()
{
    var death = FindAnyObjectByType<DeathScreen>();
    if (death != null)
    {
        int finalScore = ScoreManager.Instance.score;
        int finalWave = FindAnyObjectByType<SPAWNER>().currentWave;

        death.ShowDeathScreen("PLAYER", finalScore, finalWave);
    }

    SPAWNER.wavePaused = true;
    movement player = GetComponent<movement>();
if (player != null)
    player.canMove = false;

    gameObject.SetActive(false); // sau animație de moarte
}

}