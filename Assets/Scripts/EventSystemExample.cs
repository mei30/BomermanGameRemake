using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Example script demonstrating how to use the global game events system.
/// This script shows various ways to subscribe to events, handle them, and trigger events.
/// Use this as a reference for implementing event-based features in your game.
/// </summary>
public class EventSystemExample : MonoBehaviour
{
    [Header("UI References (Optional)")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button testButton;

    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = true;
    
    private int localScore = 0;
    private int localLives = 3;

    private void Start()
    {
        // Subscribe to various game events
        SubscribeToEvents();
        
        // Set up test button if available
        if (testButton != null)
        {
            testButton.onClick.AddListener(TriggerTestEvents);
        }

        UpdateUI();
    }

    /// <summary>
    /// Subscribe to all the events we want to handle
    /// </summary>
    private void SubscribeToEvents()
    {
        // === GAME STATE EVENTS ===
        GameEvents.OnGameStart += HandleGameStart;
        GameEvents.OnGameEnd += HandleGameEnd;
        GameEvents.OnGamePause += HandleGamePause;
        GameEvents.OnStageStart += HandleStageStart;
        GameEvents.OnStageComplete += HandleStageComplete;

        // === PLAYER EVENTS ===
        GameEvents.OnPlayerSpawn += HandlePlayerSpawn;
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
        GameEvents.OnPlayerMove += HandlePlayerMove;

        // === BOMB EVENTS ===
        GameEvents.OnBombPlaced += HandleBombPlaced;
        GameEvents.OnBombExplode += HandleBombExplode;
        GameEvents.OnBrickDestroyed += HandleBrickDestroyed;

        // === ENEMY EVENTS ===
        GameEvents.OnEnemySpawn += HandleEnemySpawn;
        GameEvents.OnEnemyDeath += HandleEnemyDeath;

        // === UI EVENTS ===
        GameEvents.OnScoreChanged += HandleScoreChanged;
        GameEvents.OnLivesChanged += HandleLivesChanged;
        GameEvents.OnTimerUpdate += HandleTimerUpdate;

        // === AUDIO EVENTS ===
        GameEvents.OnPlaySFX += HandlePlaySFX;
        GameEvents.OnPlayMusic += HandlePlayMusic;
        GameEvents.OnStopMusic += HandleStopMusic;

        // === POWER-UP EVENTS ===
        GameEvents.OnPowerUpCollected += HandlePowerUpCollected;
        GameEvents.OnBombRangeIncrease += HandleBombRangeIncrease;
        GameEvents.OnSpeedIncrease += HandleSpeedIncrease;

        if (enableDebugLogs)
            Debug.Log("[EventSystemExample] Subscribed to all game events.");
    }

    // === EVENT HANDLERS ===

    private void HandleGameStart()
    {
        if (enableDebugLogs) Debug.Log("[EventSystemExample] Game Started!");
        UpdateStatusText("Game Started!");
        localScore = 0;
        localLives = 3;
        UpdateUI();
    }

    private void HandleGameEnd()
    {
        if (enableDebugLogs) Debug.Log("[EventSystemExample] Game Ended!");
        UpdateStatusText("Game Over!");
    }

    private void HandleGamePause(bool isPaused)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Game Paused: {isPaused}");
        UpdateStatusText(isPaused ? "Game Paused" : "Game Resumed");
    }

    private void HandleStageStart(int stageNumber)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Stage {stageNumber} Started!");
        UpdateStatusText($"Stage {stageNumber} Started!");
    }

    private void HandleStageComplete(int stageNumber)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Stage {stageNumber} Completed!");
        UpdateStatusText($"Stage {stageNumber} Complete!");
    }

    private void HandlePlayerSpawn(GameObject player)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Player Spawned: {player.name}");
        UpdateStatusText("Player Spawned");
    }

    private void HandlePlayerDeath(GameObject player)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Player Died: {player.name}");
        UpdateStatusText("Player Died!");
        
        // Example: Show death effect or update UI
        StartCoroutine(ShowTemporaryMessage("You Died!", 2f));
    }

    private void HandlePlayerMove(Vector2 newPosition)
    {
        // This event fires frequently, so we'll only log occasionally
        if (enableDebugLogs && Time.frameCount % 60 == 0) // Log once per second at 60 FPS
        {
            Debug.Log($"[EventSystemExample] Player moved to: {newPosition}");
        }
    }

    private void HandleBombPlaced(GameObject bomb, Vector2 position)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Bomb placed at: {position}");
        UpdateStatusText("Bomb Placed!");
    }

    private void HandleBombExplode(GameObject bomb, Vector2 position)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Bomb exploded at: {position}");
        UpdateStatusText("BOOM!");
    }

    private void HandleBrickDestroyed(GameObject brick)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Brick destroyed: {brick?.name}");
        // Example: Add score for destroying bricks
        localScore += 10;
    }

    private void HandleEnemySpawn(GameObject enemy)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Enemy spawned: {enemy.name}");
        UpdateStatusText("Enemy Spawned!");
    }

    private void HandleEnemyDeath(GameObject enemy)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Enemy defeated: {enemy.name}");
        UpdateStatusText("Enemy Defeated!");
        // Example: Add score for defeating enemies
        localScore += 100;
    }

    private void HandleScoreChanged(int newScore)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Score changed to: {newScore}");
        localScore = newScore;
        UpdateUI();
    }

    private void HandleLivesChanged(int newLives)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Lives changed to: {newLives}");
        localLives = newLives;
        UpdateUI();
    }

    private void HandleTimerUpdate(float remainingTime)
    {
        // Timer updates frequently, so only log occasionally
        if (enableDebugLogs && Mathf.FloorToInt(remainingTime) % 10 == 0 && remainingTime % 1f < 0.1f)
        {
            Debug.Log($"[EventSystemExample] Time remaining: {remainingTime:F1}s");
        }
    }

    private void HandlePlaySFX(string soundName)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Playing SFX: {soundName}");
        // Here you would typically call your audio manager to play the sound
        // AudioManager.Instance.PlaySFX(soundName);
    }

    private void HandlePlayMusic(string musicName)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Playing Music: {musicName}");
        // Here you would typically call your audio manager to play music
        // AudioManager.Instance.PlayMusic(musicName);
    }

    private void HandleStopMusic()
    {
        if (enableDebugLogs) Debug.Log("[EventSystemExample] Stopping Music");
        // AudioManager.Instance.StopMusic();
    }

    private void HandlePowerUpCollected(GameObject powerUp, GameObject collector)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Power-up collected: {powerUp.name} by {collector.name}");
        UpdateStatusText("Power-Up Collected!");
    }

    private void HandleBombRangeIncrease(int newRange)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Bomb range increased to: {newRange}");
        UpdateStatusText($"Bomb Range: {newRange}");
    }

    private void HandleSpeedIncrease(float newSpeed)
    {
        if (enableDebugLogs) Debug.Log($"[EventSystemExample] Speed increased to: {newSpeed}");
        UpdateStatusText($"Speed Boost: {newSpeed:F1}");
    }

    // === UI HELPERS ===

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {localScore}";
        
        if (livesText != null)
            livesText.text = $"Lives: {localLives}";
    }

    private void UpdateStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
            // Clear status message after a few seconds
            CancelInvoke(nameof(ClearStatusText));
            Invoke(nameof(ClearStatusText), 3f);
        }
    }

    private void ClearStatusText()
    {
        if (statusText != null)
            statusText.text = "";
    }

    private System.Collections.IEnumerator ShowTemporaryMessage(string message, float duration)
    {
        UpdateStatusText(message);
        yield return new WaitForSeconds(duration);
        ClearStatusText();
    }

    // === TEST METHODS ===

    /// <summary>
    /// Method to test various events - can be called from a button or debug menu
    /// </summary>
    public void TriggerTestEvents()
    {
        Debug.Log("[EventSystemExample] Triggering test events...");
        
        // Test various event triggers
        EventManager.Instance.TriggerGameStart();
        EventManager.Instance.TriggerScoreChanged(localScore + 50);
        EventManager.Instance.PlaySFX("TestSound");
        
        // Test direct event invocation
        GameEvents.SafeInvoke(GameEvents.OnPlayerMove, transform.position);
        GameEvents.SafeInvoke(GameEvents.OnBombRangeIncrease, 3);
    }

    /// <summary>
    /// Example of how to trigger events from code
    /// </summary>
    [ContextMenu("Trigger Sample Events")]
    public void TriggerSampleEvents()
    {
        // Direct event system usage
        EventManager.Instance.TriggerGameStart();
        EventManager.Instance.TriggerStageStart(1);
        EventManager.Instance.PlaySFX("ExampleSound");
        EventManager.Instance.TriggerScoreChanged(500);
        
        // Using GameEvents directly with null-safe invoke
        GameEvents.SafeInvoke(GameEvents.OnPlayerMove, Vector2.zero);
        GameEvents.SafeInvoke(GameEvents.OnBombRangeIncrease, 2);
        GameEvents.SafeInvoke(GameEvents.OnSpeedIncrease, 3.5f);
    }

    // === CLEANUP ===

    private void OnDestroy()
    {
        // Always unsubscribe from events to prevent memory leaks
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        // Unsubscribe from all events
        GameEvents.OnGameStart -= HandleGameStart;
        GameEvents.OnGameEnd -= HandleGameEnd;
        GameEvents.OnGamePause -= HandleGamePause;
        GameEvents.OnStageStart -= HandleStageStart;
        GameEvents.OnStageComplete -= HandleStageComplete;
        
        GameEvents.OnPlayerSpawn -= HandlePlayerSpawn;
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
        GameEvents.OnPlayerMove -= HandlePlayerMove;
        
        GameEvents.OnBombPlaced -= HandleBombPlaced;
        GameEvents.OnBombExplode -= HandleBombExplode;
        GameEvents.OnBrickDestroyed -= HandleBrickDestroyed;
        
        GameEvents.OnEnemySpawn -= HandleEnemySpawn;
        GameEvents.OnEnemyDeath -= HandleEnemyDeath;
        
        GameEvents.OnScoreChanged -= HandleScoreChanged;
        GameEvents.OnLivesChanged -= HandleLivesChanged;
        GameEvents.OnTimerUpdate -= HandleTimerUpdate;
        
        GameEvents.OnPlaySFX -= HandlePlaySFX;
        GameEvents.OnPlayMusic -= HandlePlayMusic;
        GameEvents.OnStopMusic -= HandleStopMusic;
        
        GameEvents.OnPowerUpCollected -= HandlePowerUpCollected;
        GameEvents.OnBombRangeIncrease -= HandleBombRangeIncrease;
        GameEvents.OnSpeedIncrease -= HandleSpeedIncrease;

        if (enableDebugLogs)
            Debug.Log("[EventSystemExample] Unsubscribed from all events.");
    }
}