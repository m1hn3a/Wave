using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthSlider;

    public bool invincible = false;

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

        UpdateHealthUI();

        if (currentHealth <= 0)
            Die();
    }

    // 🔥 Funcție necesară pentru Heal Upgrade
    public void HealToFull()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        Debug.Log("HP FULL → viața a fost umplută complet!");
    }

    // 🔥 Funcție centralizată pentru UI
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
