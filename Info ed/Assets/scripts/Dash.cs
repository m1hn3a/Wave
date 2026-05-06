using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 3f;

   [HideInInspector] public bool isDashing = false;
    private bool canDash = true;

    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private movement pm;
    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
        pm = GetComponent<movement>();
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
            rb.linearVelocity = dashDirection * dashSpeed;

            if (dashTimer >= dashDuration)
            {
                EndDash();
            }
        }
    }

    void StartDash()
    {
        dashDirection = pm.lastMoveDirection;

        if (dashDirection == Vector2.zero)
            return;

        canDash = false;
        isDashing = true;
        dashTimer = 0f;

        pm.canMove = false;              // 🔥 important
        playerHealth.invincible = true;  // 🔥 invincibility ON
    }

    void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector2.zero;

        pm.canMove = true;               // 🔥 re-enable movement
        playerHealth.invincible = false; // 🔥 invincibility OFF
    }
}