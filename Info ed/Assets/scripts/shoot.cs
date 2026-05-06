using UnityEngine;
using TMPro;

public enum FireMode
{
    SingleReload,
    SemiAuto,
    FullAuto,
    TripleShot
}

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 5f;
    private float nextFire;
    private bool canShoot = true;

    public FireMode fireMode = FireMode.SingleReload;

    [Header("Laser Settings")]
    private LineRenderer lr;
    public float laserLength = 10f;

    [Header("UI")]
    public TMP_Text reloadText;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;

        lr.startColor = new Color(1f, 1f, 1f, 0.25f);
        lr.endColor   = new Color(1f, 1f, 1f, 0.05f);

        lr.enabled = true;

        if (reloadText != null)
            reloadText.gameObject.SetActive(false);
    }

    void Update()
    {
        AimAtMouse();
        UpdateLaser();
        HandleReloadInput();
        Shoot();
    }

    void AimAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - firePoint.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void UpdateLaser()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = (mousePos - firePoint.position).normalized;
        Vector3 endPos = firePoint.position + direction * laserLength;

        lr.SetPosition(0, Vector3.Lerp(lr.GetPosition(0), firePoint.position, 0.5f));
        lr.SetPosition(1, Vector3.Lerp(lr.GetPosition(1), endPos, 0.5f));
    }

    void Shoot()
    {
        switch (fireMode)
        {
            case FireMode.SingleReload:
                SingleShotMode();
                break;

            case FireMode.SemiAuto:
                SemiAutoMode();
                break;

            case FireMode.FullAuto:
                FullAutoMode();
                break;

            case FireMode.TripleShot:
                TripleShotMode();
                break;
        }
    }

    // -----------------------------
    // 🔥 MODE 0 — SINGLE + R RELOAD
    // -----------------------------
    void SingleShotMode()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            canShoot = false;

            if (reloadText != null)
                reloadText.gameObject.SetActive(true);
        }
    }

    void HandleReloadInput()
    {
        if (!canShoot && fireMode == FireMode.SingleReload)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                canShoot = true;
                reloadText.gameObject.SetActive(false);
            }
        }
    }

    // -----------------------------
    // 🔥 MODE 1 — SEMI AUTO
    // -----------------------------
    void SemiAutoMode()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    // -----------------------------
    // 🔥 MODE 2 — FULL AUTO
    // -----------------------------
    void FullAutoMode()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    // -----------------------------
    // 🔥 MODE 3 — TRIPLE SHOT
    // -----------------------------
    void TripleShotMode()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + 1f / fireRate;

            // forward
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // +45°
            Instantiate(bulletPrefab, firePoint.position,
                Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z + 45));

            // -45°
            Instantiate(bulletPrefab, firePoint.position,
                Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z - 45));
        }
    }
}
