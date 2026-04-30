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

    private bool waveActive = false;

    public bool waveStarting = false;

    // ⭐ IMPORTANT: Safe Zone logic
    public bool waveFinished = false;

    [Header("Debug Tools")]
    public bool skipWave = false;

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (skipWave)
        {
            skipWave = false;
            ForceNextWave();
        }

        // Dacă wave-ul nu e activ, nu mai facem nimic
        if (!waveActive)
            return;

        // ⭐ Când wave-ul s-a terminat
        if (enemiesSpawned >= enemiesToSpawn && enemiesAlive == 0)
        {
            waveActive = false;
            waveFinished = true; // ⭐ Safe Zone poate fi activat

            ScoreManager.Instance.comboPaused = true;

            // ❌ NU mai pornim automat următorul wave
            // ❌ NU mai folosim Invoke(StartNextWave)
        }
    }

    public void StartNextWave()
    {
        if (currentWave >= totalWaves)
            return;

        waveFinished = false; // ⭐ ieșim din Safe Zone
        waveActive = true;

        currentWave++;
        waveStarting = true;

        ScoreManager.Instance.comboPaused = true;

        if (waveText != null)
        {
            waveText.text = "Wave " + currentWave;
            waveText.gameObject.SetActive(true);
            Invoke(nameof(HideWaveText), 2f);
        }

        enemiesToSpawn = Mathf.RoundToInt(8 * Mathf.Pow(1.35f, currentWave));

        enemiesSpawned = 0;
        enemiesAlive = 0;

        StartCoroutine(SpawnWave());
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

        if (enemiesSpawned < enemiesToSpawn && enemiesAlive < 20)
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

    // ⭐ Folosit doar pentru debug
    void ForceNextWave()
    {
        foreach (var e in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(e);

        enemiesAlive = 0;
        enemiesSpawned = enemiesToSpawn;

        waveActive = false;
        waveFinished = true;

        ScoreManager.Instance.comboPaused = true;
    }
}
