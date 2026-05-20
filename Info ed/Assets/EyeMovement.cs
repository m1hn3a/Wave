using UnityEngine;

public class EnemyEyeMovement : MonoBehaviour
{
    [Header("References")]
    public GameObject player;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float acceleration = 4f;

    [Header("Hover")]
    public float hoverAmplitude = 1.5f;
    public float hoverSpeed = 2f;

    [Header("Attack")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.35f;
    public float retreatSpeed = 10f;
    public float retreatDuration = 0.4f;
    public float attackCooldown = 2f;

    private Rigidbody2D rb;

    private enum State
    {
        Hover,
        Dash,
        Retreat
    }

    private State currentState = State.Hover;

    private Vector2 moveDirection;

    private float stateTimer;
    private float cooldownTimer;

    private float hoverOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.linearDamping = 0f;

        hoverOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (player == null)
            return;

        cooldownTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Dash:

                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0f)
                {
                    StartRetreat();
                }

                break;

            case State.Retreat:

                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0f)
                {
                    currentState = State.Hover;
                }

                break;
        }

        if (currentState == State.Hover && cooldownTimer <= 0f)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        switch (currentState)
        {
            case State.Hover:
                HoverMovement();
                break;

            case State.Dash:
                rb.linearVelocity = moveDirection * dashSpeed;
                break;

            case State.Retreat:
                rb.linearVelocity = -moveDirection * retreatSpeed;
                break;
        }
    }

    void HoverMovement()
    {
        Vector2 playerPos = player.transform.position;

        float hover = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * hoverAmplitude;

        Vector2 targetPos = new Vector2(
            playerPos.x,
            playerPos.y + hover
        );

        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        rb.linearVelocity = Vector2.Lerp(
            rb.linearVelocity,
            direction * moveSpeed,
            acceleration * Time.fixedDeltaTime
        );
    }

    void StartDash()
    {
        currentState = State.Dash;

        cooldownTimer = attackCooldown;
        stateTimer = dashDuration;

        moveDirection = (
            player.transform.position - transform.position
        ).normalized;
    }

    void StartRetreat()
    {
        currentState = State.Retreat;

        stateTimer = retreatDuration;
    }
}