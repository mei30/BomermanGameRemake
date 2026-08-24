using UnityEngine;

namespace Levels
{
    /// <summary>
    /// The authoritative campaign order. Drag LevelData assets in here in play order.
    /// Both "what level comes next" and "is this level unlocked" are derived from
    /// position in this array - no need to hand-wire nextLevel/unlockRequirements
    /// on every individual LevelData asset for a straightforward linear campaign.
    /// </summary>
    [CreateAssetMenu(menuName = "Levels/Level Set")]
    public class LevelSet : ScriptableObject
    {
        public LevelData[] levels;

        public LevelData GetNext(LevelData current)
        {
            int index = System.Array.IndexOf(levels, current);
            Debug.Log($"LevelSet: GetNext({current.sceneName}) index={index}");
            if (index < 0 || index + 1 >= levels.Length) return null;
            return levels[index + 1];
        }

        public LevelData GetPrevious(LevelData current)
        {
            int index = System.Array.IndexOf(levels, current);
            if (index <= 0) return null;
            return levels[index - 1];
        }
    }
}