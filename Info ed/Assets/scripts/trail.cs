using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    public float ghostDelay = 0.01f; // 🔥 mai rapid
    private float ghostTimer;

    public GameObject ghostPrefab;
    private PlayerDash dash;

    void Start()
    {
        dash = GetComponent<PlayerDash>();
    }

    void Update()
    {
        if (dash.isDashing)
        {
            if (ghostTimer > 0)
            {
                ghostTimer -= Time.deltaTime;
            }
            else
            {
                SpawnGhost();
                ghostTimer = ghostDelay;
            }
        }
    }

    void SpawnGhost()
    {
        GameObject g = Instantiate(ghostPrefab, transform.position, transform.rotation);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer gsr = g.GetComponent<SpriteRenderer>();

        gsr.sprite = sr.sprite;
        gsr.flipX = sr.flipX;

        // 🔥 ghost vizibil mai mult timp
        Destroy(g, 0.2f);
    }
}
