using UnityEngine;

public class SlimeLook : MonoBehaviour
{
    public EnemyFollow enemy;

    public float maxTilt = 30f;

    private bool facingLeft = true;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (enemy == null || enemy.lookTarget == null)
            return;

        Vector2 dir = enemy.lookTarget.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float relativeAngle = Mathf.DeltaAngle(facingLeft ? 180f : 0f, angle);

        if (Mathf.Abs(relativeAngle) > 90f)
        {
            facingLeft = !facingLeft;

            transform.localScale = new Vector3(
                facingLeft ? originalScale.x : -originalScale.x,
                originalScale.y,
                originalScale.z
            );

            relativeAngle = Mathf.DeltaAngle(facingLeft ? 180f : 0f, angle);
        }

        float clamped = Mathf.Clamp(relativeAngle, -maxTilt, maxTilt);

        transform.rotation = Quaternion.Euler(0f, 0f, clamped);
    }
}
