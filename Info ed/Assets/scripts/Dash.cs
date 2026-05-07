using UnityEngine;

public class Dash : MonoBehaviour
{
    public bool dashUnlocked = false;
    public float dashSpeed = 8f; // valoare de start după unlock
    public float dashDuration = 0.15f;
    public float dashCooldown = 3f;

    public int dashLevel = 0; 
    public float maxDashSpeed = 22f; // LIMITA MAXIMĂ

    private bool isDashing = false;
    private float dashTimeLeft;
    private float dashCooldownLeft;

    private Rigidbody2D rb;
    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (dashCooldownLeft > 0)
            dashCooldownLeft -= Time.deltaTime;

        if (!dashUnlocked)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownLeft <= 0)
        {
            dashDirection = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;

            if (dashDirection == Vector2.zero)
                dashDirection = transform.right;

            isDashing = true;
            dashTimeLeft = dashDuration;
            dashCooldownLeft = dashCooldown;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;

            dashTimeLeft -= Time.fixedDeltaTime;

            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void UpgradeDashPower(float flatIncrease)
    {
        if (!dashUnlocked)
        {
            dashUnlocked = true;
            dashSpeed = 8f;
            Debug.Log("Dash UNLOCKED");
            return;
        }

        dashSpeed += flatIncrease;

        if (dashSpeed > maxDashSpeed)
            dashSpeed = maxDashSpeed;

        Debug.Log("Dash upgraded → " + dashSpeed);
    }
}



