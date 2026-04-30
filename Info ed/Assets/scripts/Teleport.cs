using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public SPAWNER spawner;

    [Header("Teleport Settings")]
    public float holdTime = 2f;
    private float holdTimer = 0f;

    public Transform safeZoneSpawn;
    public Transform arenaSpawn;

    private bool inSafeZone = false;
    private bool hasTeleportedThisWave = false;

    private Vector3 lastArenaPosition;

    [Header("UI")]
    public Image holdBarBG;
    public Image holdBarFill;

    void Update()
    {
        // dacă wave-ul nu e gata → nu poți ține TAB
        if (!spawner.waveFinished)
        {
            HideBar();
            return;
        }

        // dacă ești în Safe Zone → SPACE te întoarce
        if (inSafeZone)
        {
            HideBar();

            if (Input.GetKeyDown(KeyCode.Space))
                TeleportBackToArena();

            return;
        }

        // dacă ai teleportat deja în acest wave → nu mai poți
        if (hasTeleportedThisWave)
        {
            HideBar();
            return;
        }

        HandleTabHold();
    }

    void HandleTabHold()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            holdTimer += Time.deltaTime;

            ShowBar();
            holdBarFill.fillAmount = holdTimer / holdTime;

            if (holdTimer >= holdTime)
            {
                TeleportToSafeZone();
            }
        }
        else
        {
            holdTimer = 0f;
            HideBar();
        }
    }

    void TeleportToSafeZone()
    {
        hasTeleportedThisWave = true;
        inSafeZone = true;

        lastArenaPosition = transform.position;

        transform.position = safeZoneSpawn.position;
        Camera.main.GetComponent<CameraFollow>().SnapNow();

        HideBar();
    }

    void TeleportBackToArena()
    {
        inSafeZone = false;

        transform.position = lastArenaPosition;
        Camera.main.GetComponent<CameraFollow>().SnapNow();

        // pornește următorul wave
        spawner.StartNextWave();
    }

    public void ResetTeleportFlag()
    {
        hasTeleportedThisWave = false;
        holdTimer = 0f;
        HideBar();
    }

    void ShowBar()
    {
        if (holdBarBG != null) holdBarBG.enabled = true;
        if (holdBarFill != null) holdBarFill.enabled = true;
    }

    void HideBar()
    {
        if (holdBarBG != null) holdBarBG.enabled = false;
        if (holdBarFill != null) holdBarFill.enabled = false;
    }
}
