using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    [HideInInspector] public Vector2 lastMoveDirection;
    public bool canMove = true;

    public Transform playerVisual; 
    public Transform armPivot;     

    public int speedLevel = 0;

    private Rigidbody2D rb;
    private Vector2 input;

    [HideInInspector] public bool facingRight = true;

    [Header("Tilt Settings")]
    public float maxTiltAngle = 10f;
    public float tiltSpeed = 10f;

    [Header("Animation")]
    public Animator anim;

    [Header("Core Repair")]
    public Core core;               // 🔥 tragi Reactorul aici în Inspector
    public bool isRepairing = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 🔥 REPAIR LOGIC
        if (core != null && core.playerInsideRepair && Input.GetKey(KeyCode.E))
        {
            isRepairing = true;
            core.RepairCore();

            // opțional: oprești animația de mers
            anim.SetFloat("Speed", 0);
            anim.SetBool("IsMoving", false);

            return; // nu mai procesăm mișcarea
        }
        else
        {
            isRepairing = false;
        }

        // 🔥 INPUT NORMAL
        input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (input != Vector2.zero)
            lastMoveDirection = input;

        // 🔥 ANIMAȚIE
        float speed = input.magnitude;
        anim.SetFloat("Speed", speed);
        anim.SetBool("IsMoving", speed > 0.1f);

        // 🔥 FLIP
        if (input.x > 0 && !facingRight)
            Flip();
        else if (input.x < 0 && facingRight)
            Flip();

        ApplyTilt();
    }

    void FixedUpdate()
    {
        if (!canMove || isRepairing) return;

        rb.linearVelocity = input * moveSpeed;
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = playerVisual.localScale;
        scale.x *= -1;
        playerVisual.localScale = scale;
    }

    void ApplyTilt()
    {
        float targetZ = 0f;

        if (input.x > 0)
            targetZ = -maxTiltAngle;
        else if (input.x < 0)
            targetZ = maxTiltAngle;

        Quaternion targetRot = Quaternion.Euler(0, 0, targetZ);

        playerVisual.localRotation = Quaternion.Lerp(
            playerVisual.localRotation,
            targetRot,
            Time.deltaTime * tiltSpeed
        );
    }

    public void IncreaseSpeed(float flatAmount)
    {
        moveSpeed += flatAmount;
        speedLevel++;
        Debug.Log("Speed upgraded → " + moveSpeed + " | Level: " + speedLevel);
    }
}
