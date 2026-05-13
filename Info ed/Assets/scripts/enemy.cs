using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Vector2 dir = (transform.position - collision.transform.position).normalized;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(dir * bumpForce, ForceMode2D.Impulse);

            isBumped = true;
            bumpTimer = bumpDuration;
        }
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        // Knockback active
        if (isBumped)
        {
            bumpTimer -= Time.fixedDeltaTime;

            if (bumpTimer <= 0f)
            {
                isBumped = false;
            }

            return;
        }

        // Dacă e prea aproape de player
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < minDistanceFromPlayer)
        {
            Vector2 pushDir = (transform.position - player.position).normalized;
            rb.linearVelocity = pushDir * (speed * 2f);
            return;
        }

        // Move către player
        Vector2 moveDir = (player.position - transform.position).normalized;

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