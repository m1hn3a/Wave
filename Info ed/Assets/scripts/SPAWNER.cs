using UnityEngine;
using TMPro;
using System.Collections;

public class SPAWNER : MonoBehaviour
{
    public static bool wavePaused = false;
    public static bool enemiesFrozen = false;

    [Header("Main Settings")]
    public GameObject[] enemyPrefabs;
    public Transform player;
    public Transform coreTransform;

    public int totalWaves = 20;

    public TMP_Text waveText;
    public TMP_Text pauseWaveText;

    public int currentWave = 0;
    private int enemiesToSpawn = 0;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;

    public bool waveActive = false;
    public bool waveFinished = false;

    [Header("Debug")]
    public bool skipWave = false;

    void Start()
    {
        wavePaused = false;
        enemiesFrozen = false;

        waveText.gameObject.SetActive(true);
        pauseWaveText.gameObject.SetActive(false);

        StartNextWave();
    }

    void Update()
    {
        if (skipWave)
        {
            skipWave = false;

            foreach (EnemyFollow enemy in FindObjectsOfType<EnemyFollow>())
                Destroy(enemy.gameObject);

            enemiesAlive = 0;
            enemiesSpawned = enemiesToSpawn;

            EndWave();
            return;
        }

        // 🔥 FINAL DE WAVE AUTOMAT
        if (waveActive && enemiesAlive <= 0 && enemiesSpawned == enemiesToSpawn)
        {
            EndWave();
        }
    }

    void EndWave()
    {
        waveActive = false;
        waveFinished = true;

        wavePaused = true;
        enemiesFrozen = true;

        ShowPauseText();

        ScoreManager.Instance.comboPaused = true;
        FindObjectOfType<Teleport>().ResetTeleportFlag();
        TokenSystem.Instance.AddToken();
    }

    public void StartNextWave()
    {
        if (currentWave >= totalWaves)
            return;

        waveFinished = false;
        waveActive = true;
        wavePaused = false;
        enemiesFrozen = false;

        currentWave++;

        ShowWaveText("Wave " + currentWave);

        if (currentWave == 1) enemiesToSpawn = 4;
        else if (currentWave == 2) enemiesToSpawn = 7;
        else if (currentWave == 3) enemiesToSpawn = 12;
        else enemiesToSpawn = Mathf.RoundToInt(8 * Mathf.Pow(1.35f, currentWave));

        enemiesSpawned = 0;
        enemiesAlive = 0;

        StartCoroutine(UnpauseComboDelayed());
        StartCoroutine(StartWaveDelayed());
    }

    void ShowPauseText()
    {
        waveText.gameObject.SetActive(false);
        pauseWaveText.gameObject.SetActive(true);
        pauseWaveText.text = "Wave Paused";
    }

    public void ShowWaveText(string text)
    {
        pauseWaveText.gameObject.SetActive(false);
        waveText.gameObject.SetActive(true);
        waveText.text = text;
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
            if (enemiesAlive < 15)
            {
                SpawnEnemy();
                enemiesSpawned++;
                enemiesAlive++;
            }

            yield return new WaitForSeconds(1.3f);
        }
    }

    public void OnEnemyDeath()
    {
        enemiesAlive--;

        if (!waveActive)
            return;

        // 🔥 FINAL DE WAVE CORECT
        if (enemiesSpawned >= enemiesToSpawn && enemiesAlive <= 0)
        {
            EndWave();
            return;
        }

        // 🔥 Spawn continuu
        if (enemiesAlive < 15 && enemiesSpawned < enemiesToSpawn)
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

        float offset = Mathf.Clamp(6f - currentWave, 2f, 6f);

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

        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity);

        EnemyFollow follow = enemy.GetComponent<EnemyFollow>();
        EnemyDamage dmg = enemy.GetComponent<EnemyDamage>();

        follow.player = player;
        follow.coreTransform = coreTransform;
        follow.spawner = this;

        dmg.spawner = this;

        bool goToCore = (Random.value < 0.5f);
        follow.targetCore = goToCore;

        follow.ScaleWithWave(currentWave);
    }
}
