using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadarDirection : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public RectTransform radarCircle;
    public GameObject radarDotPrefab;

    [Header("Settings")]
    public float radius = 80f; // raza cercului radarului
    public float updateRate = 0.1f;
    public string enemyTag = "Enemy"; // tag-ul inamicilor din scenă

    private List<Transform> enemies = new List<Transform>();
    private List<GameObject> dots = new List<GameObject>();

    void Start()
    {
        InvokeRepeating(nameof(UpdateRadar), 0f, updateRate);
    }

    void UpdateRadar()
    {
        // curățăm lista
        foreach (GameObject dot in dots)
            Destroy(dot);
        dots.Clear();
        enemies.Clear();

        // găsim toți inamicii din scenă
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (GameObject e in foundEnemies)
        {
            enemies.Add(e.transform);
            GameObject dot = Instantiate(radarDotPrefab, radarCircle);
            dots.Add(dot);
        }

        // actualizăm pozițiile punctelor
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null) continue;

            Vector2 dir = enemies[i].position - player.position;
            dir.Normalize();

            Vector2 dotPos = dir * radius;
            dots[i].GetComponent<RectTransform>().anchoredPosition = dotPos;
        }
    }
}
