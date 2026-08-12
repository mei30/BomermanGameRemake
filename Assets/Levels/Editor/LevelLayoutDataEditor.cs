using UnityEditor;
using UnityEngine;
using Levels;

namespace Levels.Editor
{
    /// <summary>
    /// Editor-only - lives in an "Editor" folder so Unity excludes it from builds.
    /// Replaces the default flat-array Inspector with a clickable grid.
    /// Click a cell to cycle its tile type. Click "Set Player Spawn" / "Add Enemy Spawn"
    /// then click a cell to place spawn points.
    /// </summary>
    [CustomEditor(typeof(LevelLayoutData))]
    public class LevelLayoutDataEditor : UnityEditor.Editor
    {
        private const float CellSize = 24f;

        private bool _placingPlayerSpawn;
        private bool _placingEnemySpawn;

        public override void OnInspectorGUI()
        {
            var layout = (LevelLayoutData)target;

            EditorGUI.BeginChangeCheck();
            layout.width = EditorGUILayout.IntField("Width", layout.width);
            layout.height = EditorGUILayout.IntField("Height", layout.height);

            int requiredSize = layout.width * layout.height;
            if (layout.tiles == null || layout.tiles.Length != requiredSize)
            {
                if (GUILayout.Button($"Resize Tile Array to {requiredSize}"))
                {
                    var resized = new TileType[requiredSize];
                    if (layout.tiles != null)
                    {
                        for (int i = 0; i < Mathf.Min(layout.tiles.Length, requiredSize); i++)
                            resized[i] = layout.tiles[i];
                    }
                    layout.tiles = resized;
                }
                EditorGUILayout.HelpBox("Tile array size doesn't match width*height yet.", MessageType.Warning);
                if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(layout);
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("ASCII authoring: # wall, B brick, . floor, P player, E enemy", EditorStyles.miniLabel);
            layout.asciiMap = EditorGUILayout.TextArea(layout.asciiMap, GUILayout.MinHeight(120));

            if (GUILayout.Button("Generate From ASCII Map"))
            {
                layout.GenerateFromAscii();
                EditorUtility.SetDirty(layout);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Left-click: cycle tile type", EditorStyles.miniLabel);

            _placingPlayerSpawn = GUILayout.Toggle(_placingPlayerSpawn, "Click grid to set Player Spawn", "Button");
            _placingEnemySpawn = GUILayout.Toggle(_placingEnemySpawn, "Click grid to Add Enemy Spawn", "Button");

            EditorGUILayout.Space();
            DrawGrid(layout);

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(layout);
        }

        private void DrawGrid(LevelLayoutData layout)
        {
            // Draw top row (y = height-1) first so the grid reads top-to-bottom visually.
            for (int y = layout.height - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < layout.width; x++)
                {
                    int index = x + y * layout.width;
                    TileType tile = layout.tiles[index];

                    Color prevColor = GUI.backgroundColor;
                    GUI.backgroundColor = ColorForTile(tile);

                    bool isPlayerSpawn = layout.playerSpawn == new Vector2Int(x, y);
                    string label = isPlayerSpawn ? "P" : LabelForTile(tile);

                    if (GUILayout.Button(label, GUILayout.Width(CellSize), GUILayout.Height(CellSize)))
                    {
                        HandleCellClick(layout, x, y, index);
                    }

                    GUI.backgroundColor = prevColor;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void HandleCellClick(LevelLayoutData layout, int x, int y, int index)
        {
            if (_placingPlayerSpawn)
            {
                layout.playerSpawn = new Vector2Int(x, y);
                _placingPlayerSpawn = false;
                return;
            }

            if (_placingEnemySpawn)
            {
                var list = new System.Collections.Generic.List<Vector2Int>(layout.enemySpawns ?? new Vector2Int[0]);
                list.Add(new Vector2Int(x, y));
                layout.enemySpawns = list.ToArray();
                _placingEnemySpawn = false;
                return;
            }

            // Cycle Floor -> SolidWall -> Brick -> Floor
            layout.tiles[index] = (TileType)(((int)layout.tiles[index] + 1) % 3);
        }

        private Color ColorForTile(TileType tile)
        {
            switch (tile)
            {
                case TileType.Floor: return Color.white;
                case TileType.SolidWall: return Color.gray;
                case TileType.Brick: return new Color(0.8f, 0.5f, 0.3f);
                default: return Color.white;
            }
        }

        private string LabelForTile(TileType tile)
        {
            switch (tile)
            {
                case TileType.Floor: return "";
                case TileType.SolidWall: return "#";
                case TileType.Brick: return "B";
                default: return "?";
            }
        }
    }
}