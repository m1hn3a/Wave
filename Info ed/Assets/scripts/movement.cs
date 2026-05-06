using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    [HideInInspector] public Vector2 lastMoveDirection;
    public bool canMove = true; // 🔥 adăugat

    private Rigidbody2D rb;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (input != Vector2.zero)
            lastMoveDirection = input;
    }

    void FixedUpdate()
    {
        if (!canMove) return; // 🔥 FIX IMPORTANT

        rb.linearVelocity = input * moveSpeed;
    }
}