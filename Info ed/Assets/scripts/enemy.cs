using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;

    public float avoidanceRadius = 1f;
    public float avoidanceStrength = 1.5f;
    public float wobbleStrength = 0.5f;

    public SPAWNER spawner;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        Vector2 moveDir = (player.position - transform.position).normalized;

        Vector2 wobble = new Vector2(
            Mathf.PerlinNoise(Time.time * 1.2f, transform.position.x) - 0.5f,
            Mathf.PerlinNoise(Time.time * 1.2f, transform.position.y) - 0.5f
        ) * wobbleStrength;

        moveDir += wobble;

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
