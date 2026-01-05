using UnityEngine;

/// <summary>
/// Handles game rules and responds to game events during a level.
/// Listens to events like player death, enemy death, and manages game flow accordingly.
/// Each level should have its own GameFlow instance.
/// </summary>
public class GameFlow : MonoBehaviour
{
    private void Awake()
    {
        // No singleton - each level gets its own GameFlow instance
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        // Subscribe to game events
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
        GameEvents.OnEnemyDeath += HandleEnemyDeath;
        GameEvents.OnBombPlaced += HandleBombPlaced;
        GameEvents.OnBombExplode += HandleBombExplode;
        GameEvents.OnBrickDestroyed += HandleBrickDestroyed;
        GameEvents.OnPlayerSpawn += HandlePlayerSpawn;
        GameEvents.OnEnemySpawn += HandleEnemySpawn;
        GameEvents.OnStageComplete += HandleStageComplete;
    }

    private void UnsubscribeFromEvents()
    {
        // Unsubscribe from events to prevent memory leaks
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
        GameEvents.OnEnemyDeath -= HandleEnemyDeath;
        GameEvents.OnBombPlaced -= HandleBombPlaced;
        GameEvents.OnBombExplode -= HandleBombExplode;
        GameEvents.OnBrickDestroyed -= HandleBrickDestroyed;
        GameEvents.OnPlayerSpawn -= HandlePlayerSpawn;
        GameEvents.OnEnemySpawn -= HandleEnemySpawn;
        GameEvents.OnStageComplete -= HandleStageComplete;
    }

    // Event Handlers
    private void HandlePlayerDeath(GameObject player)
    {
        Debug.Log($"[GameFlow] Player died: {player?.name}");
    }

    private void HandleEnemyDeath(GameObject enemy)
    {
        Debug.Log($"[GameFlow] Enemy died: {enemy?.name}");
    }

    private void HandleBombPlaced(GameObject bomb, Vector2 position)
    {
        Debug.Log($"[GameFlow] Bomb placed at {position}");
    }

    private void HandleBombExplode(GameObject bomb, Vector2 position)
    {
        Debug.Log($"[GameFlow] Bomb exploded at {position}");
    }

    private void HandleBrickDestroyed(GameObject brick)
    {
        Debug.Log($"[GameFlow] Brick destroyed: {brick?.name}");
    }

    private void HandlePlayerSpawn(GameObject player)
    {
        Debug.Log($"[GameFlow] Player spawned: {player?.name}");
    }

    private void HandleEnemySpawn(GameObject enemy)
    {
        Debug.Log($"[GameFlow] Enemy spawned: {enemy?.name}");
    }

    private void HandleStageComplete(int stageNumber)
    {
        Debug.Log($"[GameFlow] Stage {stageNumber} completed!");
    }
}
