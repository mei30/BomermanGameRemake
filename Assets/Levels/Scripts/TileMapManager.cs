using Unity.Cinemachine;
using UnityEngine;
using Levels;

namespace Levels
{
    /// <summary>
    /// Lives INSIDE each level scene (like LevelObjective) - listens for OnLevelLoaded,
    /// then reads LevelManager.CurrentLevel.layout and builds the pitch: tiles, player,
    /// enemies. Torn down automatically when the level scene unloads.
    /// </summary>
    public class TileMapManager : MonoBehaviour
    {
        [Header("Tile Prefabs")]
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject solidWallPrefab;
        [SerializeField] private GameObject brickPrefab;

        [Header("Entity Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject enemyPrefab;

        [Header("Grid Settings")]
        [SerializeField] private float tileSize = 1f;

        void Awake()
        {

            LevelLayoutData layout = LevelManager.Instance.CurrentLevel?.layout;
            if (layout == null)
            {
                Debug.LogWarning("TileMapManager: current level has no layout assigned.");
                return;
            }

            BuildTiles(layout);
            SpawnPlayer(layout);
            SpawnEnemies(layout);
            Debug.Log("TileMapManager: Awake called, registering for level loaded event");
        }

        void Start()
        {
            Debug.Log("TileMapManager: Start called, waiting for level to load");
        }
        private void HandleLevelLoaded()
        {
        }

        private void BuildTiles(LevelLayoutData layout)
        {
            for (int y = 0; y < layout.height; y++)
            {
                for (int x = 0; x < layout.width; x++)
                {
                    TileType tile = layout.GetTile(x, y);
                    GameObject prefab = PrefabForTile(tile);
                    if (prefab == null) continue;

                    Debug.Log("Tile Manger: " + x + ", " + y);
                    Vector3 worldPos = GridToWorld(x, y);
                    Instantiate(prefab, worldPos, Quaternion.identity, transform);
                }
            }
        }

        private GameObject PrefabForTile(TileType tile)
        {
            switch (tile)
            {
                case TileType.Floor: return floorPrefab;
                case TileType.SolidWall: return solidWallPrefab;
                case TileType.Brick: return brickPrefab;
                default: return null;
            }
        }

        private void SpawnPlayer(LevelLayoutData layout)
        {
            if (playerPrefab == null) return;

            Vector3 worldPos = GridToWorld(layout.playerSpawn.x, layout.playerSpawn.y);
            GameObject player = Instantiate(playerPrefab, worldPos, Quaternion.identity);

            CameraManager.Instance.SetTarget(player.transform);
            Debug.Log("TileMapManager: Spawned player at " + worldPos);
        }

        private void SpawnEnemies(LevelLayoutData layout)
        {
            if (enemyPrefab == null || layout.enemySpawns == null) return;

            foreach (var spawn in layout.enemySpawns)
            {
                Vector3 worldPos = GridToWorld(spawn.x, spawn.y);
                Instantiate(enemyPrefab, worldPos, Quaternion.identity);
            }
        }

        private Vector3 GridToWorld(int x, int y)
        {
            return new Vector3(x * tileSize, y * tileSize, 0.0f);
        }
    }
}