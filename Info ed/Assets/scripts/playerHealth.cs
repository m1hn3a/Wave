using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    // 🔥 Flash roșu
    void FlashRed()
    {
        if (playerSprite != null)
            StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        Color original = playerSprite.color;

        playerSprite.color = new Color(1f, 0.3f, 0.3f);
        yield return new WaitForSeconds(flashDuration);
        playerSprite.color = original;
    }

    // 🔥 Damage SFX
    void PlayDamageSFX()
    {
        if (audioSource != null && damageSFX != null)
        {
            audioSource.PlayOneShot(damageSFX, sfxVolume);
        }
    }

    // 🔥 Heal Upgrade
    public void HealToFull()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        Debug.Log("HP FULL → viața a fost umplută complet!");
    }

    // 🔥 UI centralizat
    public void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    void Die()
    {
        Debug.Log("Player died");

        EnemyDamage[] enemies = FindObjectsOfType<EnemyDamage>();
        foreach (EnemyDamage e in enemies)
        {
            e.FreezeEnemy();
        }

        Destroy(gameObject);
    }
}
