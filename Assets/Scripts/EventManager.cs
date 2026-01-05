using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central event manager that provides additional functionality for event management,
/// including event logging, debugging, and persistent event subscriptions.
/// This complements the static GameEvents class with instance-based management.
/// </summary>
public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<EventManager>();
                if (_instance == null)
                {
                    GameObject eventManagerObject = new GameObject("EventManager");
                    _instance = eventManagerObject.AddComponent<EventManager>();
                    DontDestroyOnLoad(eventManagerObject);
                }
            }
            return _instance;
        }
    }

    [Header("Debug Settings")]
    [SerializeField] private bool enableEventLogging = false;
    [SerializeField] private bool logToConsole = true;
    [SerializeField] private bool logToFile = false;

    [Header("Event Statistics")]
    [SerializeField] private bool trackEventCalls = false;
    
    // Event statistics tracking
    private Dictionary<string, int> eventCallCounts = new Dictionary<string, int>();
    private Dictionary<string, float> lastEventTimes = new Dictionary<string, float>();
    
    // Event history for debugging
    private Queue<EventLogEntry> eventHistory = new Queue<EventLogEntry>();
    private const int MAX_EVENT_HISTORY = 100;

    private struct EventLogEntry
    {
        public string eventName;
        public float timestamp;
        public string parameters;

        public EventLogEntry(string name, float time, string param = "")
        {
            eventName = name;
            timestamp = time;
            parameters = param;
        }
    }

    private void Awake()
    {
        // Singleton pattern
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeEventManager();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeEventManager()
    {
        Debug.Log("[EventManager] Event Manager initialized and ready.");
        
        // Subscribe to key events for logging if enabled
        if (enableEventLogging)
        {
            SubscribeToAllEventsForLogging();
        }
    }

    private void SubscribeToAllEventsForLogging()
    {
        // Game State Events
        GameEvents.OnGameStart += () => LogEvent("OnGameStart");
        GameEvents.OnGameEnd += () => LogEvent("OnGameEnd");
        GameEvents.OnGamePause += (paused) => LogEvent("OnGamePause", $"paused: {paused}");
        GameEvents.OnStageStart += (stage) => LogEvent("OnStageStart", $"stage: {stage}");
        GameEvents.OnStageComplete += (stage) => LogEvent("OnStageComplete", $"stage: {stage}");
        GameEvents.OnReturnToMainMenu += () => LogEvent("OnReturnToMainMenu");

        // Player Events
        GameEvents.OnPlayerSpawn += (player) => LogEvent("OnPlayerSpawn", $"player: {player?.name}");
        GameEvents.OnPlayerDeath += (player) => LogEvent("OnPlayerDeath", $"player: {player?.name}");
        GameEvents.OnPlayerMove += (pos) => LogEvent("OnPlayerMove", $"position: {pos}");
        GameEvents.OnPlayerPlaceBomb += (pos) => LogEvent("OnPlayerPlaceBomb", $"position: {pos}");

        // Bomb & Explosion Events
        GameEvents.OnBombPlaced += (bomb, pos) => LogEvent("OnBombPlaced", $"bomb: {bomb?.name}, position: {pos}");
        GameEvents.OnBombExplode += (bomb, pos) => LogEvent("OnBombExplode", $"bomb: {bomb?.name}, position: {pos}");
        GameEvents.OnExplosionFlameSpawn += (pos, duration) => LogEvent("OnExplosionFlameSpawn", $"position: {pos}, duration: {duration}");
        GameEvents.OnBrickDestroyed += (brick) => LogEvent("OnBrickDestroyed", $"brick: {brick?.name}");

        // Enemy Events
        GameEvents.OnEnemySpawn += (enemy) => LogEvent("OnEnemySpawn", $"enemy: {enemy?.name}");
        GameEvents.OnEnemyDeath += (enemy) => LogEvent("OnEnemyDeath", $"enemy: {enemy?.name}");
        GameEvents.OnEnemyMove += (enemy, pos) => LogEvent("OnEnemyMove", $"enemy: {enemy?.name}, position: {pos}");

        // UI Events
        GameEvents.OnScoreChanged += (score) => LogEvent("OnScoreChanged", $"score: {score}");
        GameEvents.OnLivesChanged += (lives) => LogEvent("OnLivesChanged", $"lives: {lives}");
        GameEvents.OnTimerUpdate += (time) => LogEvent("OnTimerUpdate", $"time: {time:F1}");

        // Audio Events
        GameEvents.OnPlaySFX += (sfx) => LogEvent("OnPlaySFX", $"sound: {sfx}");
        GameEvents.OnPlayMusic += (music) => LogEvent("OnPlayMusic", $"music: {music}");
        GameEvents.OnStopMusic += () => LogEvent("OnStopMusic");
    }

    /// <summary>
    /// Trigger game start event with logging
    /// </summary>
    public void TriggerGameStart()
    {
        GameEvents.SafeInvoke(GameEvents.OnGameStart);
    }

    /// <summary>
    /// Trigger game end event with logging
    /// </summary>
    public void TriggerGameEnd()
    {
        GameEvents.SafeInvoke(GameEvents.OnGameEnd);
    }

    /// <summary>
    /// Trigger game pause event with logging
    /// </summary>
    public void TriggerGamePause(bool isPaused)
    {
        GameEvents.SafeInvoke(GameEvents.OnGamePause, isPaused);
    }

    /// <summary>
    /// Trigger stage start event with logging
    /// </summary>
    public void TriggerStageStart(int stageNumber)
    {
        GameEvents.SafeInvoke(GameEvents.OnStageStart, stageNumber);
    }

    /// <summary>
    /// Trigger stage complete event with logging
    /// </summary>
    public void TriggerStageComplete(int stageNumber)
    {
        GameEvents.SafeInvoke(GameEvents.OnStageComplete, stageNumber);
    }

    /// <summary>
    /// Trigger player death event with logging
    /// </summary>
    public void TriggerPlayerDeath(GameObject player)
    {
        GameEvents.SafeInvoke(GameEvents.OnPlayerDeath, player);
    }

    /// <summary>
    /// Trigger player spawn event with logging
    /// </summary>
    public void TriggerPlayerSpawn(GameObject player)
    {
        GameEvents.SafeInvoke(GameEvents.OnPlayerSpawn, player);
    }

    /// <summary>
    /// Trigger bomb placed event with logging
    /// </summary>
    public void TriggerBombPlaced(GameObject bomb, Vector2 position)
    {
        GameEvents.SafeInvoke(GameEvents.OnBombPlaced, bomb, position);
        GameEvents.SafeInvoke(GameEvents.OnPlayerPlaceBomb, position);
    }

    /// <summary>
    /// Trigger bomb explode event with logging
    /// </summary>
    public void TriggerBombExplode(GameObject bomb, Vector2 position)
    {
        GameEvents.SafeInvoke(GameEvents.OnBombExplode, bomb, position);
    }

    /// <summary>
    /// Trigger score change event with logging
    /// </summary>
    public void TriggerScoreChanged(int newScore)
    {
        GameEvents.SafeInvoke(GameEvents.OnScoreChanged, newScore);
    }

    /// <summary>
    /// Trigger enemy death event with logging
    /// </summary>
    public void TriggerEnemyDeath(GameObject enemy)
    {
        GameEvents.SafeInvoke(GameEvents.OnEnemyDeath, enemy);
    }

    /// <summary>
    /// Trigger brick destroyed event with logging
    /// </summary>
    public void TriggerBrickDestroyed(GameObject brick)
    {
        GameEvents.SafeInvoke(GameEvents.OnBrickDestroyed, brick);
    }

    /// <summary>
    /// Play sound effect through event system
    /// </summary>
    public void PlaySFX(string soundName)
    {
        GameEvents.SafeInvoke(GameEvents.OnPlaySFX, soundName);
    }

    /// <summary>
    /// Play music through event system
    /// </summary>
    public void PlayMusic(string musicName)
    {
        GameEvents.SafeInvoke(GameEvents.OnPlayMusic, musicName);
    }

    /// <summary>
    /// Stop music through event system
    /// </summary>
    public void StopMusic()
    {
        GameEvents.SafeInvoke(GameEvents.OnStopMusic);
    }

    private void LogEvent(string eventName, string parameters = "")
    {
        if (!enableEventLogging) return;

        float currentTime = Time.time;
        
        // Track event statistics
        if (trackEventCalls)
        {
            if (eventCallCounts.ContainsKey(eventName))
                eventCallCounts[eventName]++;
            else
                eventCallCounts[eventName] = 1;
            
            lastEventTimes[eventName] = currentTime;
        }

        // Add to event history
        EventLogEntry entry = new EventLogEntry(eventName, currentTime, parameters);
        eventHistory.Enqueue(entry);
        
        // Keep history within limits
        if (eventHistory.Count > MAX_EVENT_HISTORY)
            eventHistory.Dequeue();

        // Log to console
        if (logToConsole)
        {
            string logMessage = $"[EventManager] {eventName}";
            if (!string.IsNullOrEmpty(parameters))
                logMessage += $" - {parameters}";
            
            Debug.Log(logMessage);
        }

        // Log to file (if implemented)
        if (logToFile)
        {
            // Could implement file logging here if needed
            // LogToFile(eventName, parameters, currentTime);
        }
    }

    /// <summary>
    /// Get statistics for a specific event
    /// </summary>
    public int GetEventCallCount(string eventName)
    {
        return eventCallCounts.ContainsKey(eventName) ? eventCallCounts[eventName] : 0;
    }

    /// <summary>
    /// Get the last time a specific event was called
    /// </summary>
    public float GetLastEventTime(string eventName)
    {
        return lastEventTimes.ContainsKey(eventName) ? lastEventTimes[eventName] : -1f;
    }

    /// <summary>
    /// Get recent event history for debugging
    /// </summary>
    // public EventLogEntry[] GetRecentEvents(int count = 10)
    // {
    //     EventLogEntry[] events = eventHistory.ToArray();
    //     int startIndex = Mathf.Max(0, events.Length - count);
    //     int actualCount = Mathf.Min(count, events.Length);
        
    //     EventLogEntry[] result = new EventLogEntry[actualCount];
    //     Array.Copy(events, startIndex, result, 0, actualCount);
        
    //     return result;
    // }

    /// <summary>
    /// Clear all event statistics and history
    /// </summary>
    public void ClearEventData()
    {
        eventCallCounts.Clear();
        lastEventTimes.Clear();
        eventHistory.Clear();
        Debug.Log("[EventManager] Event data cleared.");
    }

    /// <summary>
    /// Enable or disable event logging at runtime
    /// </summary>
    public void SetEventLogging(bool enabled)
    {
        if (enabled && !enableEventLogging)
        {
            enableEventLogging = true;
            SubscribeToAllEventsForLogging();
            Debug.Log("[EventManager] Event logging enabled.");
        }
        else if (!enabled && enableEventLogging)
        {
            enableEventLogging = false;
            Debug.Log("[EventManager] Event logging disabled.");
        }
    }

    private void OnDestroy()
    {
        // Clean up when destroyed
        if (_instance == this)
        {
            _instance = null;
        }
    }

    // Editor/Debug helpers
    #if UNITY_EDITOR
    [ContextMenu("Print Event Statistics")]
    private void PrintEventStatistics()
    {
        Debug.Log("=== Event Call Statistics ===");
        foreach (var kvp in eventCallCounts)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value} calls (last: {lastEventTimes[kvp.Key]:F2}s)");
        }
    }

    // [ContextMenu("Print Recent Events")]
    // private void PrintRecentEvents()
    // {
    //     Debug.Log("=== Recent Events ===");
    //     var recentEvents = GetRecentEvents();
    //     for (int i = 0; i < recentEvents.Length; i++)
    //     {
    //         var evt = recentEvents[i];
    //         string logStr = $"{evt.timestamp:F2}s: {evt.eventName}";
    //         if (!string.IsNullOrEmpty(evt.parameters))
    //             logStr += $" ({evt.parameters})";
    //         Debug.Log(logStr);
    //     }
    // }

    [ContextMenu("Clear Event Data")]
    private void ClearEventDataEditor()
    {
        ClearEventData();
    }
    #endif
}