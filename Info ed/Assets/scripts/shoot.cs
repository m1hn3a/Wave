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

    public int bulletPierce = 1;

    [Header("Ammo System")]
    public int currentAmmo;
    public int maxAmmo;
    public float reloadTime = 2.5f;
    private bool isReloading = false;

    [Header("Laser Settings")]
    private LineRenderer lr;
    public float laserLength = 10f;

    [Header("UI")]
    public TMP_Text ammoText;
    public RectTransform reloadIcon;   // 🔥 iconița de glonț

    public FireMode fireMode = FireMode.SingleReload;

    void Start()
{
    lr = GetComponent<LineRenderer>();
    lr.positionCount = 2;

    lr.startColor = new Color(1f, 1f, 1f, 0.25f);
    lr.endColor   = new Color(1f, 1f, 1f, 0.05f);

    lr.enabled = true;

    // 🔥 Iconița este vizibilă de la început
    if (reloadIcon != null)
    {
        reloadIcon.gameObject.SetActive(true);
        reloadIcon.localRotation = Quaternion.Euler(0, 0, -90f); // poziția normală
    }

    SetAmmoForMode();
    UpdateAmmoUI();
}


    void Update()
    {
        AimAtMouse();
        UpdateLaser();

        if (!isReloading)
            Shoot();

        if (currentAmmo <= 0 && !isReloading)
            StartCoroutine(Reload());
    }

    // ---------------------------------------------------------
    // PUBLIC — necesar pentru upgrade-uri
    // ---------------------------------------------------------
    public void SetAmmoForMode()
    {
        switch (fireMode)
        {
            case FireMode.SingleReload:
                maxAmmo = 1;
                break;

            case FireMode.SemiAuto:
                maxAmmo = 7;
                break;

            case FireMode.FullAuto:
                maxAmmo = 20;
                break;

            case FireMode.TripleShot:
                maxAmmo = 15;
                break;
        }

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    // ---------------------------------------------------------
    // 🔥 ANIMAȚIA DE RELOAD CU ICONIȚĂ
    // ---------------------------------------------------------
    System.Collections.IEnumerator Reload()
{
    isReloading = true;

    if (reloadIcon != null)
    {
        reloadIcon.gameObject.SetActive(true);
        reloadIcon.localRotation = Quaternion.Euler(0, 0, -90f); // poziția normală
    }

    float timer = 0f;

    // 0 → 1 sec: rotire -90° → +90° (adică 180° total)
    while (timer < 1f)
    {
        timer += Time.deltaTime;
        float t = timer / 1f;
        float angle = Mathf.Lerp(-90f, 90f, t);

        if (reloadIcon != null)
            reloadIcon.localRotation = Quaternion.Euler(0, 0, angle);

        yield return null;
    }

    // 1 → 1.5 sec: pauză la 90°
    yield return new WaitForSeconds(0.5f);

    // 1.5 → 2.5 sec: rotire 90° → 270° (adică înapoi la -90°)
    timer = 0f;
    while (timer < 1f)
    {
        timer += Time.deltaTime;
        float t = timer / 1f;
        float angle = Mathf.Lerp(90f, 270f, t); // 270° = -90° vizual

        if (reloadIcon != null)
            reloadIcon.localRotation = Quaternion.Euler(0, 0, angle);

        yield return null;
    }

    // Final reload
    currentAmmo = maxAmmo;
    isReloading = false;

    if (reloadIcon != null)
    {
        reloadIcon.localRotation = Quaternion.Euler(0, 0, -90f); // poziția normală
        reloadIcon.gameObject.SetActive(true); // rămâne vizibilă
    }

    UpdateAmmoUI();
}

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + "/" + maxAmmo;
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
    // MODE 0 — SINGLE SHOT (1 glonț)
    // ---------------------------------------------------------
    void SingleShotMode()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            ShootBullet();
            currentAmmo--;
            UpdateAmmoUI();
        }
    }

    // ---------------------------------------------------------
    // MODE 1 — SEMI AUTO (7 gloanțe)
    // ---------------------------------------------------------
    void SemiAutoMode()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire && currentAmmo > 0)
        {
            nextFire = Time.time + 1f / fireRate;
            ShootBullet();
            currentAmmo--;
            UpdateAmmoUI();
        }
    }

    // ---------------------------------------------------------
    // MODE 2 — FULL AUTO (20 gloanțe)
    // ---------------------------------------------------------
    void FullAutoMode()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire && currentAmmo > 0)
        {
            nextFire = Time.time + 1f / fireRate;
            ShootBullet();
            currentAmmo--;
            UpdateAmmoUI();
        }
    }

    // ---------------------------------------------------------
    // MODE 3 — TRIPLE SHOT (15 gloanțe)
    // ---------------------------------------------------------
    void TripleShotMode()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire && currentAmmo > 0)
        {
            nextFire = Time.time + 1f / fireRate;

            // bullet 1
            ShootBullet();
            currentAmmo--;
            UpdateAmmoUI();

            // bullet 2
            if (currentAmmo > 0)
            {
                GameObject b1 = Instantiate(bulletPrefab, firePoint.position,
                    Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z + 45));
                b1.GetComponent<Bullet>().pierce = bulletPierce;
                currentAmmo--;
                UpdateAmmoUI();
            }

            // bullet 3
            if (currentAmmo > 0)
            {
                GameObject b2 = Instantiate(bulletPrefab, firePoint.position,
                    Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z - 45));
                b2.GetComponent<Bullet>().pierce = bulletPierce;
                currentAmmo--;
                UpdateAmmoUI();
            }
        }
    }

    void ShootBullet()
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        b.GetComponent<Bullet>().pierce = bulletPierce;
    }
}
