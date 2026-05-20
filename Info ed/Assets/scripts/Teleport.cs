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
        // 🔥 În tutorial → teleport nelimitat
        if (SPAWNER.tutorialMode)
        {
            TutorialTeleportLogic();
            return;
        }

        // 🔥 Normal game logic
        if (!SPAWNER.waveFinished)
        {
            HideBar();
            return;
        }

        if (inSafeZone)
        {
            HideBar();

            if (Input.GetKeyDown(KeyCode.Space))
                TeleportBackToArena();

            return;
        }

        if (hasTeleportedThisWave)
        {
            HideBar();
            return;
        }

        HandleTabHold();
    }

    // 🔥 LOGICĂ SPECIALĂ PENTRU TUTORIAL
    void TutorialTeleportLogic()
    {
        // Dacă ești în SafeZone → SPACE te întoarce
        if (inSafeZone)
        {
            HideBar();

            if (Input.GetKeyDown(KeyCode.Space))
                TeleportBackToArena();

            return;
        }

        // Dacă ești în arenă → TAB te duce în SafeZone
        HandleTabHold(); // fără limită, fără waveFinished
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

        if (!SPAWNER.tutorialMode)
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
