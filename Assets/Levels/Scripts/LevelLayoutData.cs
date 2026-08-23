using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    /// <summary>
    /// The "pitch" - grid layout, tiles, and spawn points for one level.
    /// Separate asset from LevelData so the same layout could theoretically be reused
    /// across multiple LevelData entries (e.g. same map, different difficulty/enemy count),
    /// and so TileMapManager only depends on this, not all of LevelData's metadata.
    /// </summary>
    [CreateAssetMenu(menuName = "Levels/Level Layout")]
    public class LevelLayoutData : ScriptableObject
    {
        [Header("Grid Size")]
        public int width = 13;
        public int height = 31;

        [Header("Tile Grid")]
        // Flattened 1D array, index = x + y * width. Fill it either via the custom grid
        // Inspector, or by writing asciiMap below and clicking "Generate From ASCII".
        public TileType[] tiles;

        [Header("Spawn Points (grid coordinates, not world position)")]
        public Vector2Int playerSpawn;
        public Vector2Int[] enemySpawns;

        [Header("ASCII Map (optional authoring source)")]
        // Legend: '#' = SolidWall, 'B' = Brick, '.' = Floor, 'P' = player spawn (also Floor),
        // 'E' = enemy spawn (also Floor). One line per row. Top line of the text = top row (y = height-1).
        // Call GenerateFromAscii() (or the Editor button) to (re)populate width/height/tiles/spawns from this.
        [TextArea(4, 20)]
        public string asciiMap;

        public TileType GetTile(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return TileType.SolidWall;
            return tiles[x + y * width];
        }

        /// <summary>
        /// Parses asciiMap into width/height/tiles/playerSpawn/enemySpawns.
        /// Overwrites whatever was there before - ASCII is treated as the source of truth
        /// when this is called.
        /// </summary>
        public void GenerateFromAscii()
        {
            if (string.IsNullOrWhiteSpace(asciiMap)) return;

            // Split on newlines, trim trailing empty lines, ignore carriage returns.
            string[] rawLines = asciiMap.Replace("\r", "").Split('\n');
            var lines = new List<string>();
            foreach (var line in rawLines)
            {
                if (line.Length > 0) lines.Add(line);
            }
            if (lines.Count == 0) return;

            height = lines.Count;
            width = 0;
            foreach (var line in lines) width = Mathf.Max(width, line.Length);

            tiles = new TileType[width * height];
            var enemySpawnList = new List<Vector2Int>();

            // First line in the text = TOP row = highest y value.
            for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                int y = height - 1 - lineIndex;
                string line = lines[lineIndex];

                for (int x = 0; x < width; x++)
                {
                    char c = x < line.Length ? line[x] : '.';
                    int index = x + y * width;

                    switch (c)
                    {
                        case '#':
                            tiles[index] = TileType.SolidWall;
                            break;
                        case 'B':
                            tiles[index] = TileType.Brick;
                            break;
                        case 'P':
                            tiles[index] = TileType.Floor;
                            playerSpawn = new Vector2Int(x, y);
                            break;
                        case 'E':
                            tiles[index] = TileType.Floor;
                            enemySpawnList.Add(new Vector2Int(x, y));
                            break;
                        case '.':
                        default:
                            tiles[index] = TileType.Floor;
                            break;
                    }
                }
            }

            enemySpawns = enemySpawnList.ToArray();
        }
    }
}