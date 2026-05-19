using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public SPAWNER spawner;

    [Header("Damage Settings")]
    public int damage = 10;
    public float damageCooldown = 1f;

    private float lastDamageTime = 0f;

    [Header("Death Particles")]
    public GameObject deathParticles;

    private EnemyFollow followScript;
    private Rigidbody2D rb;

    private bool isDead = false;

    void Awake()
    {
        followScript = GetComponent<EnemyFollow>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth hp = collision.collider.GetComponent<PlayerHealth>();

            if (hp != null && Time.time >= lastDamageTime + damageCooldown)
            {
                hp.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }

    public void Die(Vector2 hitDirection)
    {
        if (isDead) return;
        isDead = true;

        if (deathParticles != null)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, -10f);
            GameObject p = Instantiate(deathParticles, spawnPos, Quaternion.identity);

            float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;
            p.transform.rotation = Quaternion.Euler(0, 0, angle);

            ParticleSystem ps = p.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                float totalDuration = ps.main.duration + ps.main.startLifetime.constantMax;
                Destroy(p, totalDuration);
            }
            else
            {
                Destroy(p, 5f);
            }
        }

        ScoreManager.Instance.AddKill(10);

        if (spawner != null)
            spawner.OnEnemyDeath();

        Destroy(gameObject);
    }

    public void Die()
    {
        Die(Vector2.right);
    }
}
