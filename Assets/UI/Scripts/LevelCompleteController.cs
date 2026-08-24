using UnityEngine;
using UnityEngine.SceneManagement;
using Levels;

namespace UI
{
    /// <summary>
    /// Lives on the Canvas inside the LevelComplete scene (loaded additively on top of
    /// the level scene, see LevelCompleteState.Enter()).
    /// </summary>
    public class LevelCompleteController : MonoBehaviour
    {
        public void OnContinueClicked()
        {
            LevelData current = LevelManager.Instance.CurrentLevel;
            LevelData next = current;

            if (next == null)
            {
                // No next level defined - nothing to continue to, fall back to quitting.
                Debug.LogWarning("LevelCompleteController: No next level defined, quitting to main menu.");
                OnQuitClicked();
                return;
            }

            // Order matters: change state first so LevelComplete scene unloads (via
            // LevelCompleteState.Exit()), THEN load the next level.
            GameManager.Instance.StartGame();
        }

        public void OnQuitClicked()
        {
            // Unload the level scene we were playing, then return to the menu.
            // LevelManager doesn't currently track "unload just the level" separately from
            // "load a new one" - simplest correct approach: go back to MainMenu, whose
            // state Enter()/Exit() can be extended to also unload the current level scene
            // if one is still loaded. For now, explicit unload here:
            if (LevelManager.Instance.CurrentLevel != null)
            {
                SceneManager.UnloadSceneAsync(LevelManager.Instance.CurrentLevel.sceneName);
            }

            // GameManager.Instance.ChangeState(GameManager.Instance.MainMenu);
        }
    }
}