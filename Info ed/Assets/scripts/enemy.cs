using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;

    public float avoidanceRadius = 1f;
    public float avoidanceStrength = 1.5f;
    public float wobbleStrength = 0.5f;

public SPAWNER spawner;

    public float bumpForce = 10f;          // cât de tare sunt împinși
    public float bumpDuration = 0.12f;     // cât durează knockback-ul
    public float minDistanceFromPlayer = 0.7f; // distanța minimă permisă

    private bool isBumped = false;
    private Vector2 bumpVelocity;
    private float bumpTimer = 0f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Knockback instant
            Vector2 dir = (transform.position - other.transform.position).normalized;
            bumpVelocity = dir * bumpForce;
            bumpTimer = bumpDuration;
            isBumped = true;
        }
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        // Dacă e în knockback, nu urmărește playerul
        if (isBumped)
        {
            rb.linearVelocity = bumpVelocity;
            bumpTimer -= Time.fixedDeltaTime;

            if (bumpTimer <= 0)
                isBumped = false;

            return;
        }

        // Dacă e prea aproape de player, se dă înapoi automat
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < minDistanceFromPlayer)
        {
            Vector2 pushDir = (transform.position - player.position).normalized;
            rb.linearVelocity = pushDir * (speed * 2f);
            return;
        }

        // Mișcare normală spre player
        Vector2 moveDir = (player.position - transform.position).normalized;

        // Wobble
        Vector2 wobble = new Vector2(
            Mathf.PerlinNoise(Time.time * 1.2f, transform.position.x) - 0.5f,
            Mathf.PerlinNoise(Time.time * 1.2f, transform.position.y) - 0.5f
        ) * wobbleStrength;

        moveDir += wobble;

        // Avoidance între inamici
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
