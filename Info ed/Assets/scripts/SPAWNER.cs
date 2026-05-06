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

    public TMP_Text waveText; // 🔥 mereu activ

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
        waveText.gameObject.SetActive(true); // 🔥 mereu activ
        StartNextWave();
    }

    void Update()
    {
        // SKIP WAVE
        if (skipWave)
        {
            skipWave = false;

            foreach (EnemyFollow enemy in FindObjectsOfType<EnemyFollow>())
                Destroy(enemy.gameObject);

            enemiesAlive = 0;
            enemiesSpawned = enemiesToSpawn;

            waveActive = false;
            waveFinished = true;

            ScoreManager.Instance.comboPaused = true;
            FindObjectOfType<Teleport>().ResetTeleportFlag();

            TokenSystem.Instance.AddToken(); // token la skip
            return;
        }

        // WAVE TERMINAT NORMAL
        if (waveActive && enemiesAlive <= 0 && enemiesSpawned == enemiesToSpawn)
        {
            waveActive = false;
            waveFinished = true;

            ScoreManager.Instance.comboPaused = true;
            FindObjectOfType<Teleport>().ResetTeleportFlag();

            TokenSystem.Instance.AddToken(); // token la final
        }

        if (!waveActive)
            return;
    }

    public void StartNextWave()
    {
        if (currentWave >= totalWaves)
            return;

        waveFinished = false;
        waveActive = true;

        currentWave++;

        // 🔥 wave text actualizat și mereu vizibil
        waveText.text = "Wave " + currentWave;

        enemiesToSpawn = Mathf.RoundToInt(8 * Mathf.Pow(1.35f, currentWave));
        enemiesSpawned = 0;
        enemiesAlive = 0;

        StartCoroutine(UnpauseComboDelayed());
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
