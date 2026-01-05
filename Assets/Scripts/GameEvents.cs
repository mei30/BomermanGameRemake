using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Static class containing all global game events.
/// This provides a centralized location for all game events and their data types.
/// </summary>
public static class GameEvents
{
    // === GAME STATE EVENTS ===
    
    /// <summary>Event triggered when the game starts</summary>
    public static Action OnGameStart;
    
    /// <summary>Event triggered when the game ends</summary>
    public static Action OnGameEnd;
    
    /// <summary>Event triggered when the game is paused</summary>
    public static Action<bool> OnGamePause; // bool = isPaused
    
    /// <summary>Event triggered when transitioning to next stage</summary>
    public static Action<int> OnStageStart; // int = stage number
    
    /// <summary>Event triggered when a stage is completed</summary>
    public static Action<int> OnStageComplete; // int = stage number
    
    /// <summary>Event triggered when returning to main menu</summary>
    public static Action OnReturnToMainMenu;

    // === PLAYER EVENTS ===
    
    /// <summary>Event triggered when player spawns</summary>
    public static Action<GameObject> OnPlayerSpawn; // GameObject = player instance
    
    /// <summary>Event triggered when player dies</summary>
    public static Action<GameObject> OnPlayerDeath; // GameObject = player instance
    
    /// <summary>Event triggered when player moves</summary>
    public static Action<Vector2> OnPlayerMove; // Vector2 = new position
    
    /// <summary>Event triggered when player places a bomb</summary>
    public static Action<Vector2> OnPlayerPlaceBomb; // Vector2 = bomb position

    // === BOMB & EXPLOSION EVENTS ===
    
    /// <summary>Event triggered when a bomb is placed</summary>
    public static Action<GameObject, Vector2> OnBombPlaced; // GameObject = bomb, Vector2 = position
    
    /// <summary>Event triggered when a bomb explodes</summary>
    public static Action<GameObject, Vector2> OnBombExplode; // GameObject = bomb, Vector2 = position
    
    /// <summary>Event triggered when an explosion flame spawns</summary>
    public static Action<Vector2, float> OnExplosionFlameSpawn; // Vector2 = position, float = duration
    
    /// <summary>Event triggered when an explosion destroys a brick</summary>
    public static Action<GameObject> OnBrickDestroyed; // GameObject = destroyed brick

    // === ENEMY EVENTS ===
    
    /// <summary>Event triggered when an enemy spawns</summary>
    public static Action<GameObject> OnEnemySpawn; // GameObject = enemy instance
    
    /// <summary>Event triggered when an enemy dies</summary>
    public static Action<GameObject> OnEnemyDeath; // GameObject = enemy instance
    
    /// <summary>Event triggered when an enemy moves</summary>
    public static Action<GameObject, Vector2> OnEnemyMove; // GameObject = enemy, Vector2 = new position

    // === POWER-UP EVENTS ===
    
    /// <summary>Event triggered when a power-up spawns</summary>
    public static Action<GameObject, Vector2> OnPowerUpSpawn; // GameObject = power-up, Vector2 = position
    
    /// <summary>Event triggered when a power-up is collected</summary>
    public static Action<GameObject, GameObject> OnPowerUpCollected; // GameObject = power-up, GameObject = collector
    
    /// <summary>Event triggered when player gains bomb range increase</summary>
    public static Action<int> OnBombRangeIncrease; // int = new range
    
    /// <summary>Event triggered when player gains bomb count increase</summary>
    public static Action<int> OnBombCountIncrease; // int = new max bombs
    
    /// <summary>Event triggered when player gains speed increase</summary>
    public static Action<float> OnSpeedIncrease; // float = new speed

    // === UI EVENTS ===
    
    /// <summary>Event triggered when score changes</summary>
    public static Action<int> OnScoreChanged; // int = new score
    
    /// <summary>Event triggered when lives change</summary>
    public static Action<int> OnLivesChanged; // int = new lives count
    
    /// <summary>Event triggered when timer updates</summary>
    public static Action<float> OnTimerUpdate; // float = remaining time
    
    /// <summary>Event triggered when showing/hiding UI panels</summary>
    public static Action<string, bool> OnUIPanel; // string = panel name, bool = show/hide

    // === AUDIO EVENTS ===
    
    /// <summary>Event triggered to play a sound effect</summary>
    public static Action<string> OnPlaySFX; // string = sound effect name
    
    /// <summary>Event triggered to play background music</summary>
    public static Action<string> OnPlayMusic; // string = music track name
    
    /// <summary>Event triggered to stop current music</summary>
    public static Action OnStopMusic;
    
    /// <summary>Event triggered when audio volume changes</summary>
    public static Action<float> OnVolumeChange; // float = new volume (0-1)

    // === LEVEL EVENTS ===
    
    /// <summary>Event triggered when level generation starts</summary>
    public static Action OnLevelGenerationStart;
    
    /// <summary>Event triggered when level generation completes</summary>
    public static Action OnLevelGenerationComplete;
    
    /// <summary>Event triggered when a tile is destroyed</summary>
    public static Action<Vector2> OnTileDestroyed; // Vector2 = tile position
    
    /// <summary>Event triggered when a tile is spawned</summary>
    public static Action<GameObject, Vector2> OnTileSpawned; // GameObject = tile, Vector2 = position

    // === UTILITY METHODS ===
    
    /// <summary>
    /// Clear all event subscriptions. Use with caution!
    /// This should typically only be called when completely resetting the game state.
    /// </summary>
    public static void ClearAllEvents()
    {
        // Game State Events
        OnGameStart = null;
        OnGameEnd = null;
        OnGamePause = null;
        OnStageStart = null;
        OnStageComplete = null;
        OnReturnToMainMenu = null;

        // Player Events
        OnPlayerSpawn = null;
        OnPlayerDeath = null;
        OnPlayerMove = null;
        OnPlayerPlaceBomb = null;

        // Bomb & Explosion Events
        OnBombPlaced = null;
        OnBombExplode = null;
        OnExplosionFlameSpawn = null;
        OnBrickDestroyed = null;

        // Enemy Events
        OnEnemySpawn = null;
        OnEnemyDeath = null;
        OnEnemyMove = null;

        // Power-up Events
        OnPowerUpSpawn = null;
        OnPowerUpCollected = null;
        OnBombRangeIncrease = null;
        OnBombCountIncrease = null;
        OnSpeedIncrease = null;

        // UI Events
        OnScoreChanged = null;
        OnLivesChanged = null;
        OnTimerUpdate = null;
        OnUIPanel = null;

        // Audio Events
        OnPlaySFX = null;
        OnPlayMusic = null;
        OnStopMusic = null;
        OnVolumeChange = null;

        // Level Events
        OnLevelGenerationStart = null;
        OnLevelGenerationComplete = null;
        OnTileDestroyed = null;
        OnTileSpawned = null;
    }

    /// <summary>
    /// Safe way to invoke an Action event with null checking
    /// </summary>
    public static void SafeInvoke(Action action)
    {
        action?.Invoke();
    }

    /// <summary>
    /// Safe way to invoke an Action event with one parameter
    /// </summary>
    public static void SafeInvoke<T>(Action<T> action, T parameter)
    {
        action?.Invoke(parameter);
    }

    /// <summary>
    /// Safe way to invoke an Action event with two parameters
    /// </summary>
    public static void SafeInvoke<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2)
    {
        action?.Invoke(param1, param2);
    }
}