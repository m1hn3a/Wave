using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

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
                // Calculate the bullet's travel direction
                Vector2 hitDirection = transform.right; 
                
                // Call the correct Die() function
                enemy.Die(hitDirection);
            }

            Destroy(gameObject); // destroy bullet
        }
    }
}