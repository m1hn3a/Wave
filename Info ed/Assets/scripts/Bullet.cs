using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    // 🔥 câți inamici poate lovi glonțul
    public int pierce = 1;

    void Start()
    {
        Destroy(gameObject, 5f); // despawn after 5 seconds
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyDamage enemy = col.GetComponent<EnemyDamage>();
            if (enemy != null)
            {
                // direcția glonțului
                Vector2 hitDirection = transform.right;

                // omoară inamicul
                enemy.Die(hitDirection);
            }

            // 🔥 scade pierce
            pierce--;

            // dacă nu mai are pierce → distruge glonțul
            if (pierce <= 0)
                Destroy(gameObject);
        }
    }
}
