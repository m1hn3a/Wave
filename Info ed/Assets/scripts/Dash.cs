using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 3f;

    private bool isDashing = false;
    private bool canDash = true;

    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // cooldown
        if (!canDash)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= dashCooldown)
            {
                canDash = true;
                cooldownTimer = 0f;
            }
        }

        // input
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartDash();
        }

        // dash movement
        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            rb.velocity = dashDirection * dashSpeed;

            if (dashTimer >= dashDuration)
            {
                EndDash();
            }
        }
    }

  void StartDash()
{
    // luăm direcția
    dashDirection = new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
    ).normalized;

    // dacă nu te miști → NU dăm dash
    if (dashDirection == Vector2.zero)
    {
        return;
    }

    canDash = false;
    isDashing = true;
    dashTimer = 0f;

    // invincibilitate ON
    playerHealth.invincible = true;
}
    void EndDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero;

        // invincibilitate OFF
        playerHealth.invincible = false;
    }
}
