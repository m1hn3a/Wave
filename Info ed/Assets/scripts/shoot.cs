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

    // 🔥 Bullet piercing (upgrade)
    public int bulletPierce = 1;

    // SINGLE SHOT COOLDOWN
    private float singleShotCooldown = 2.5f; 
    private float singleShotTimer = 0f;
    private bool canShoot = true;
    private bool cooldownFinished = false;

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

    // ---------------------------------------------------------
    // 🔥 MODE 0 — SINGLE SHOT + R RELOAD + 2.5s COOLDOWN FIXED
    // ---------------------------------------------------------
    void SingleShotMode()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            ShootBullet();
            canShoot = false;
            cooldownFinished = false;
            singleShotTimer = singleShotCooldown;

            if (reloadText != null)
                reloadText.gameObject.SetActive(true);
        }

        if (!canShoot)
        {
            singleShotTimer -= Time.deltaTime;

            if (singleShotTimer <= 0)
                cooldownFinished = true;

            if (cooldownFinished && Input.GetKeyDown(KeyCode.R))
            {
                canShoot = true;
                cooldownFinished = false;

                if (reloadText != null)
                    reloadText.gameObject.SetActive(false);
            }
        }
    }

    // ---------------------------------------------------------
    // 🔥 MODE 1 — SEMI AUTO
    // ---------------------------------------------------------
    void SemiAutoMode()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            ShootBullet();
        }
    }

    // ---------------------------------------------------------
    // 🔥 MODE 2 — FULL AUTO
    // ---------------------------------------------------------
    void FullAutoMode()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            ShootBullet();
        }
    }

    // ---------------------------------------------------------
    // 🔥 MODE 3 — TRIPLE SHOT
    // ---------------------------------------------------------
    void TripleShotMode()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + 1f / fireRate;

            ShootBullet();

            GameObject b1 = Instantiate(bulletPrefab, firePoint.position,
                Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z + 45));
            b1.GetComponent<Bullet>().pierce = bulletPierce;

            GameObject b2 = Instantiate(bulletPrefab, firePoint.position,
                Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z - 45));
            b2.GetComponent<Bullet>().pierce = bulletPierce;
        }
    }

    // ---------------------------------------------------------
    // 🔥 FUNCȚIE CENTRALĂ PENTRU TRAS
    // ---------------------------------------------------------
    void ShootBullet()
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        b.GetComponent<Bullet>().pierce = bulletPierce;
    }
}
