using UnityEngine;

public class RepairZone : MonoBehaviour
{
    public Core core;   // 🔥 tragi reactorul aici în Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        core.playerInsideRepair = true;

        // încetinim playerul
        movement m = other.GetComponent<movement>();
        if (m != null)
            m.moveSpeed *= 0.75f;

        // blocăm tragerea
        PlayerShoot ps = other.GetComponent<PlayerShoot>();
        if (ps != null)
            ps.canShoot = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        core.playerInsideRepair = false;

        // revenim la viteza normală
        movement m = other.GetComponent<movement>();
        if (m != null)
            m.moveSpeed /= 0.75f;

        // reactivăm tragerea
        PlayerShoot ps = other.GetComponent<PlayerShoot>();
        if (ps != null)
            ps.canShoot = true;
    }
}
