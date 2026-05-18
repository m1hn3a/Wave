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
    public RectTransform reloadIcon;

    [Header("Arm Rotation")]
    public Transform armPivot;

    [Header("Repair Lock")]
    public bool canShoot = true;   // 🔥 ADĂUGAT

    public FireMode fireMode = FireMode.SingleReload;

    private movement move;

    void Start()
    {
        move = FindAnyObjectByType<movement>();

        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;

        lr.startColor = new Color(1f, 1f, 1f, 0.25f);
        lr.endColor   = new Color(1f, 1f, 1f, 0.05f);

        lr.enabled = true;

        if (reloadIcon != null)
        {
            reloadIcon.gameObject.SetActive(true);
            reloadIcon.localRotation = Quaternion.Euler(0, 0, -90f);
        }

        SetAmmoForMode();
        UpdateAmmoUI();
    }

    void Update()
    {
        AimArmAtMouse();
        UpdateLaser();

        if (!isReloading)
            Shoot();

        if (currentAmmo <= 0 && !isReloading)
            StartCoroutine(Reload());
    }

    void AimArmAtMouse()
    {
        if (armPivot == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - armPivot.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (!move.facingRight)
            angle += 180f;

        armPivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    void UpdateLaser()
    {
        if (armPivot == null || firePoint == null) return;

        Vector3 direction = armPivot.right;

        if (!move.facingRight)
            direction = -direction;

        Vector3 startPos = firePoint.position;
        Vector3 endPos = startPos + direction * laserLength;

        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
    }

    public void SetAmmoForMode()
    {
        switch (fireMode)
        {
            case FireMode.SingleReload: maxAmmo = 1; break;
            case FireMode.SemiAuto:     maxAmmo = 7; break;
            case FireMode.FullAuto:     maxAmmo = 20; break;
            case FireMode.TripleShot:   maxAmmo = 15; break;
        }

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        if (reloadIcon != null)
        {
            reloadIcon.gameObject.SetActive(true);
            reloadIcon.localRotation = Quaternion.Euler(0, 0, -90f);
        }

        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime;
            float t = timer / 1f;
            float angle = Mathf.Lerp(-90f, 90f, t);

            if (reloadIcon != null)
                reloadIcon.localRotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            float t = timer / 1f;
            float angle = Mathf.Lerp(90f, 270f, t);

            if (reloadIcon != null)
                reloadIcon.localRotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        }

        currentAmmo = maxAmmo;
        isReloading = false;

        if (reloadIcon != null)
        {
            reloadIcon.localRotation = Quaternion.Euler(0, 0, -90f);
            reloadIcon.gameObject.SetActive(true);
        }

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + "/" + maxAmmo;
    }

    void Shoot()
    {
        if (!canShoot) return; // 🔥 BLOCĂM TRAGEREA ÎN ZONA DE REPAIR

        switch (fireMode)
        {
            case FireMode.SingleReload: SingleShotMode(); break;
            case FireMode.SemiAuto:     SemiAutoMode(); break;
            case FireMode.FullAuto:     FullAutoMode(); break;
            case FireMode.TripleShot:   TripleShotMode(); break;
        }
    }

    void SingleShotMode()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            ShootBullet();
            currentAmmo--;
            UpdateAmmoUI();
        }
    }

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

    void TripleShotMode()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire && currentAmmo > 0)
        {
            nextFire = Time.time + 1f / fireRate;

            ShootBullet();
            currentAmmo--;
            UpdateAmmoUI();

            if (currentAmmo > 0)
            {
                GameObject b1 = Instantiate(bulletPrefab, firePoint.position,
                    Quaternion.Euler(0, 0, firePoint.rotation.eulerAngles.z + 45));
                b1.GetComponent<Bullet>().pierce = bulletPierce;
                currentAmmo--;
                UpdateAmmoUI();
            }

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
        Quaternion rot = firePoint.rotation;

        if (!move.facingRight)
            rot = Quaternion.Euler(0, 0, rot.eulerAngles.z + 180f);

        GameObject b = Instantiate(bulletPrefab, firePoint.position, rot);
        b.GetComponent<Bullet>().pierce = bulletPierce;
    }
}
