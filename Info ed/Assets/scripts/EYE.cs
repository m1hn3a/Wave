using UnityEngine;

public class EYE : MonoBehaviour
{
    public EnemyFollow enemy;

    void Update()
    {
        if (enemy == null || enemy.lookTarget == null)
            return;

        Vector2 dir = enemy.lookTarget.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
