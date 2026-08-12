using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Win: survive until the timer runs out. Lose: player dies before then.
    /// Different rules entirely from KillCountObjective, but LevelManager and the
    /// event channels don't need to know or care which one is active in a given level.
    /// </summary>
    public class SurviveTimeObjective : LevelObjectiveBase
    {
        [Header("Event Channels")]
        [SerializeField] private GameEvent onPlayerDied;

        private void OnEnable()
        {
            onPlayerDied?.RegisterListener(HandlePlayerDied);
        }

        private void OnDisable()
        {
            onPlayerDied?.UnregisterListener(HandlePlayerDied);
        }

        private void HandlePlayerDied()
        {
            FailLevel();
        }
    }
}