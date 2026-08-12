using UnityEngine;

namespace Levels
{
    /// <summary>
    /// One asset per level. Create via Assets > Create > Levels > Level Data.
    /// Adding a new level = creating a new asset, not writing new code.
    /// </summary>
    [CreateAssetMenu(menuName = "Levels/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Identity")]
        public string levelId;          // unique key used for save data, e.g. "level_01"
        public string displayName;
        [TextArea] public string description;

        [Header("Scene")]
        public string sceneName;        // name of the additively-loaded scene for this level

        [Header("Progression")]
        public int orderIndex;          // position in level select
        public LevelData[] unlockRequirements; // levels that must be completed first

        [Header("Scoring / Difficulty")]
        public int parScore;
        public float parTime;
        public Difficulty difficulty;

        [Header("Presentation")]
        public Sprite thumbnail;
        public AudioClip musicClip;     // song to play while this level is active

        [Header("Layout")]
        public LevelLayoutData layout;  // the pitch - grid, tiles, spawn points
    }

    public enum Difficulty { Easy, Medium, Hard, Expert }
}