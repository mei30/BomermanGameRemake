using UnityEngine;
using Unity.Cinemachine;

public class GridManager : MonoBehaviour
{
    public int width = 31;
    public int height = 13;

    public float tileSize = 1f;

    public GameObject playerPrefab;   // drag Bomberman prefab here
    public GameObject baloonPrefab;

    public GameObject floorTilePrefab;
    public GameObject wallTilePrefab;
    public GameObject brickTilePrefab;

    public Vector2Int spawnPosition = new Vector2Int(1, 1);

    public Vector2Int baloonSpawnPosition = new Vector2Int(1, 23);
    public Vector2Int baloonSpawnPosition2 = new Vector2Int(37, 1);
    public Vector2Int baloonSpawnPosition3 = new Vector2Int(37, 23);

    private GameObject playerInstance;

    [Range(0f, 1f)]
    public float brickSpawnChance = 0.3f; // 60% chance to spawn a breakable brick

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateLevel();
        SpawnPlayer();
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (baloonPrefab == null)
        {
            Debug.LogError("Baloon Prefab not assigned!");
            return;
        }

        Vector3 worldPos1 = new Vector3(baloonSpawnPosition.x, baloonSpawnPosition.y, 0);
        Vector3 worldPos2 = new Vector3(baloonSpawnPosition2.x, baloonSpawnPosition2.y, 0);
        Vector3 worldPos3 = new Vector3(baloonSpawnPosition3.x, baloonSpawnPosition3.y, 0);
        playerInstance = Instantiate(baloonPrefab, worldPos1, Quaternion.identity);
        // playerInstance = Instantiate(baloonPrefab, worldPos2, Quaternion.identity);
        // playerInstance = Instantiate(baloonPrefab, worldPos3, Quaternion.identity);

    }

    void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab not assigned!");
            return;
        }

        // Convert grid coords → world position
        Vector3 worldPos = new Vector3(spawnPosition.x, spawnPosition.y, 0);
        playerInstance = Instantiate(playerPrefab, worldPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateLevel()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * tileSize, y * tileSize, 0);
                Instantiate(floorTilePrefab, pos, Quaternion.identity);

                // Example: outer walls
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    Instantiate(wallTilePrefab, pos, Quaternion.identity);
                // Example: inner checkerboard walls
                else if (x % 2 == 0 && y % 2 == 0)
                    Instantiate(wallTilePrefab, pos, Quaternion.identity);
                //Else place breakable bricks randomly (later)
                else
                {
                    if ((Random.value < brickSpawnChance) && (y != 1 && x != 1)
                            && (y != 2 && x != 1) && (y != 1 && x != 2) && (y != 23) && (x != 37))
                        Instantiate(brickTilePrefab, pos, Quaternion.identity);

                }
            }
        }
    }
}
