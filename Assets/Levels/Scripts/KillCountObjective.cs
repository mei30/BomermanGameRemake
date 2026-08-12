using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Win: clear all enemies. Lose: player dies.
    /// Drop this on a "LevelController" GameObject inside the level scene,
    /// assign the shared OnEnemyKilled / OnPlayerDied channels, set the enemy count.
    /// </summary>
    public class KillCountObjective : LevelObjectiveBase
    {
        [Header("Event Channels")]
        [SerializeField] private GameEvent onEnemyKilled;
        [SerializeField] private GameEvent onPlayerDied;

        [Header("Config")]
        [SerializeField] private int totalEnemies = 5;
        [SerializeField] private int scorePerEnemy = 100;

        private int _enemiesRemaining;

        protected override void Awake()
        {
            base.Awake();
            _enemiesRemaining = totalEnemies;
        }

        private void OnEnable()
        {
            onEnemyKilled?.RegisterListener(HandleEnemyKilled);
            onPlayerDied?.RegisterListener(HandlePlayerDied);
        }

        private void OnDisable()
        {
            onEnemyKilled?.UnregisterListener(HandleEnemyKilled);
            onPlayerDied?.UnregisterListener(HandlePlayerDied);
        }

        private void HandleEnemyKilled()
        {
            Score += scorePerEnemy;
            _enemiesRemaining--;

            if (_enemiesRemaining <= 0)
            {
                CompleteLevel();
            }
        }

        private void HandlePlayerDied()
        {
            FailLevel();
        }
    }
}