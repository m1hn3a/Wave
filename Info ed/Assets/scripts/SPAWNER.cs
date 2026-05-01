using UnityEngine;
using TMPro;
using System.Collections;

public class SPAWNER : MonoBehaviour
{
    [Header("Main Settings")]
    public GameObject enemyPrefab;
    public Transform player;

    public int totalWaves = 20;
    public float spawnInterval = 0.1f;

    public TMP_Text waveText;

    private int currentWave = 0;
    private int enemiesToSpawn = 0;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;

    public bool waveActive = false;
    public bool waveFinished = false;
    [Header("Debug")]
public bool skipWave = false;


    void Start()
    {
        StartNextWave();
    }

   void Update()
{
Debug.Log("Alive: " + enemiesAlive + " | Finished: " + waveFinished);


    if (skipWave)
{
    skipWave = false;

    // distrugem toți inamicii existenți
    foreach (EnemyFollow enemy in FindObjectsOfType<EnemyFollow>())
        Destroy(enemy.gameObject);

    enemiesAlive = 0;
    enemiesSpawned = enemiesToSpawn;

    waveActive = false;
    waveFinished = true;

    // înghețăm combo-ul ca la final de wave
    ScoreManager.Instance.comboPaused = true;

    // ⭐ resetăm teleportul ca să meargă TAB
    FindObjectOfType<Teleport>().ResetTeleportFlag();

    return;
}

if (waveActive && enemiesAlive <= 0 && enemiesSpawned == enemiesToSpawn)
{
    waveActive = false;
    waveFinished = true;
    ScoreManager.Instance.comboPaused = true;

    // resetăm teleportul
    FindObjectOfType<Teleport>().ResetTeleportFlag();
}


        if (!waveActive)
            return;

        if (enemiesSpawned >= enemiesToSpawn && enemiesAlive <= 0)
        {
            waveActive = false;
            waveFinished = true;
            ScoreManager.Instance.comboPaused = true;
        }
    }



    public void StartNextWave()
    {
        if (currentWave >= totalWaves)
            return;

        waveFinished = false;
        waveActive = true;

        currentWave++;

        if (waveText != null)
        {
            waveText.text = "Wave " + currentWave;
            waveText.gameObject.SetActive(true);
            Invoke(nameof(HideWaveText), 2f);
        }

        enemiesToSpawn = Mathf.RoundToInt(8 * Mathf.Pow(1.35f, currentWave));
        enemiesSpawned = 0;
        enemiesAlive = 0;

        // ⭐ combo înghețat încă 0.4 secunde
        StartCoroutine(UnpauseComboDelayed());

        // ⭐ întârziere 0.2 secunde înainte de spawn
        StartCoroutine(StartWaveDelayed());
    }

    IEnumerator StartWaveDelayed()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(SpawnWave());
    }

    IEnumerator UnpauseComboDelayed()
    {
        ScoreManager.Instance.comboPaused = true;
        yield return new WaitForSeconds(0.4f);
        ScoreManager.Instance.comboPaused = false;
    }

    void HideWaveText()
    {
        waveText.gameObject.SetActive(false);
    }

    IEnumerator SpawnWave()
    {
        while (enemiesSpawned < enemiesToSpawn)
        {
            if (enemiesAlive < 20)
            {
                SpawnEnemy();
                enemiesSpawned++;
                enemiesAlive++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void OnEnemyDeath()
    {
        enemiesAlive--;

        if (!waveActive)
            return;

        if (enemiesSpawned >= enemiesToSpawn)
            return;

        if (enemiesAlive < 20)
        {
            SpawnEnemy();
            enemiesSpawned++;
            enemiesAlive++;
        }
    }

    void SpawnEnemy()
    {
        Camera cam = Camera.main;

        Vector2 screenMin = cam.ScreenToWorldPoint(new Vector3(0, 0));
        Vector2 screenMax = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        float offset = 2f;
        Vector2 spawnPos = Vector2.zero;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0:
                spawnPos = new Vector2(screenMin.x - offset, Random.Range(screenMin.y, screenMax.y));
                break;

            case 1:
                spawnPos = new Vector2(screenMax.x + offset, Random.Range(screenMin.y, screenMax.y));
                break;

            case 2:
                spawnPos = new Vector2(Random.Range(screenMin.x, screenMax.x), screenMin.y - offset);
                break;

            case 3:
                spawnPos = new Vector2(Random.Range(screenMin.x, screenMax.x), screenMax.y + offset);
                break;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        EnemyFollow follow = enemy.GetComponent<EnemyFollow>();
        EnemyDamage dmg = enemy.GetComponent<EnemyDamage>();

        follow.player = player;
        follow.spawner = this;
        dmg.spawner = this;
    }
}
