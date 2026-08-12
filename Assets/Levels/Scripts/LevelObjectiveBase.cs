using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Base for any per-level win/lose logic. Lives inside a level scene, not Core -
    /// gets torn down automatically when the level unloads.
    ///
    /// LevelManager only ever sees the OUTCOME (CompleteCurrentLevel / FailCurrentLevel),
    /// never the rules that produced it. That's what lets Level_03 use
    /// SurviveTimeObjective and Level_07 use KillCountObjective without LevelManager
    /// changing at all.
    /// </summary>
    public abstract class LevelObjectiveBase : MonoBehaviour
    {
        protected float LevelStartTime { get; private set; }
        protected int Score { get; set; }

        protected virtual void Awake()
        {
            LevelStartTime = Time.time;
        }

        protected void CompleteLevel()
        {
            float elapsed = Time.time - LevelStartTime;
            LevelManager.Instance.LevelComplete();
        }

        protected void FailLevel()
        {
            LevelManager.Instance.LevelFailed();
        }
    }
}