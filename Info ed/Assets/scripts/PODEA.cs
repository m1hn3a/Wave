using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    [Header("Tile Settings")]
    public GameObject[] floorTiles;   // 0 = comun, 1-2 = rare
    public int width = 20;
    public int height = 20;

    [Header("Scale Settings")]
    public float tileScale = 2f;

    private float autoTileSize;
    private Vector2 offset;

    public GameObject GameManager; // tragi GameManager-ul aici

    void Start()
    {
        CalculateTileSize();
        CalculateCenterOffset();
        GenerateFloor();
        
    }

    void CalculateTileSize()
    {
        if (floorTiles.Length == 0)
        {
            Debug.LogError("Nu ai tile-uri în listă!");
            return;
        }

        SpriteRenderer sr = floorTiles[0].GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Debug.LogError("Tile-urile trebuie să aibă SpriteRenderer!");
            return;
        }

        float spriteWidthUnits = sr.sprite.bounds.size.x;
        autoTileSize = spriteWidthUnits * tileScale;
    }

    void CalculateCenterOffset()
    {
        float totalWidth = width * autoTileSize;
        float totalHeight = height * autoTileSize;

        offset = new Vector2(
            -totalWidth / 2f + autoTileSize / 2f,
            -totalHeight / 2f + autoTileSize / 2f
        );
    }

    void GenerateFloor()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tilePrefab = ChooseTile();

                Vector3 pos = new Vector3(
                    x * autoTileSize + offset.x,
                    y * autoTileSize + offset.y,
                    1f // AICI SETĂM Z = 1
                );

                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tile.transform.localScale = Vector3.one * tileScale;
            }
        }
    }

    GameObject ChooseTile()
    {
        float r = Random.value;

        if (r < 0.80f)
            return floorTiles[0];

        if (r < 0.90f)
            return floorTiles[1];

        return floorTiles[2];
    }
}
