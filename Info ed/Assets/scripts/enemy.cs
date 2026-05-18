using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public Transform coreTransform;
    public bool targetCore = false;

    public float speed = 2f;

    public float avoidanceRadius = 1f;
    public float avoidanceStrength = 1.5f;
    public float wobbleStrength = 0.5f;

    public SPAWNER spawner;

    [Header("Knockback")]
    public float bumpForce = 10f;
    public float bumpDuration = 0.2f;
    public float minDistanceFromPlayer = 0.7f;

    private bool isBumped = false;
    private float bumpTimer = 0f;

    private Rigidbody2D rb;

    [Header("Health")]
    public int baseHP = 10;
    public int currentHP;

    [HideInInspector] public Transform lookTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ScaleWithWave(int wave)
    {
        float multiplier = 1f + wave * 0.2f;
        currentHP = Mathf.RoundToInt(baseHP * multiplier);
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            spawner.OnEnemyDeath();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // PLAYER knockback
        if (collision.collider.CompareTag("Player"))
        {
            Vector2 dir = (transform.position - collision.transform.position).normalized;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(dir * bumpForce, ForceMode2D.Impulse);

            isBumped = true;
            bumpTimer = bumpDuration;
        }

        // CORE damage + knockback
        if (collision.collider.CompareTag("Core"))
        {
            Vector2 dir = (transform.position - collision.transform.position).normalized;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(dir * bumpForce, ForceMode2D.Impulse);

            isBumped = true;
            bumpTimer = bumpDuration;

            Core core = collision.collider.GetComponent<Core>();
            if (core != null)
                core.TakeDamage(core.damagePerHit);
        }
    }

    void FixedUpdate()
    {
        // 🔥 FREEZE INSTANT când moare playerul sau reactorul
        if (SPAWNER.enemiesFrozen)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (player == null)
            return;

        if (isBumped)
        {
            bumpTimer -= Time.fixedDeltaTime;

            if (bumpTimer <= 0f)
                isBumped = false;

            return;
        }

        // Ținta
        Transform core = coreTransform != null ? coreTransform : GameObject.FindGameObjectWithTag("Core").transform;
        Vector2 targetPos = targetCore ? core.position : player.position;

        lookTarget = targetCore ? core : player;

        // Prea aproape de player → împinge înapoi
        if (!targetCore)
        {
            float dist = Vector2.Distance(transform.position, player.position);

            if (dist < minDistanceFromPlayer)
            {
                Vector2 pushDir = (transform.position - player.position).normalized;
                rb.linearVelocity = pushDir * (speed * 2f);
                return;
            }
        }

        // Direcția principală
        Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;

        // Wobble
        Vector2 wobble = new Vector2(
            Mathf.PerlinNoise(Time.time * 1.2f, transform.position.x) - 0.5f,
            Mathf.PerlinNoise(Time.time * 1.2f, transform.position.y) - 0.5f
        ) * wobbleStrength;

        moveDir += wobble;

        // Avoidance
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius);

        foreach (var col in nearby)
        {
            if (col.gameObject != gameObject && col.CompareTag("Enemy"))
            {
                Vector2 away = (Vector2)(transform.position - col.transform.position);
                moveDir += away.normalized * avoidanceStrength;
            }
        }

        rb.linearVelocity = moveDir.normalized * speed;
    }
}
