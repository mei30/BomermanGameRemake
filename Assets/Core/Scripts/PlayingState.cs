using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayingState : IState
{
    private const string GameSceneName = "GameScene";

    public void Enter()
    {
        Debug.Log("Entering Playing State");

        if (LevelManager.Instance == null)
        {
            Debug.LogError("PlayingState: LevelManager instance is missing.");
            return;
        }

        LevelManager.Instance.LoadGameScene();
    }

    public void Exit()
    {
        Debug.Log("Exiting Playing State");

        if (LevelManager.Instance == null)
        {
            Debug.LogError("PlayingState: LevelManager instance is missing, cannot unload GameScene.");
            return;
        }

        LevelManager.Instance.StartCoroutine(UnloadGameSceneAfterDelay());
    }

    public void Update()
    {
        // Game playing update logic here
    }

    private IEnumerator UnloadGameSceneAfterDelay()
    {
        Scene gameScene = SceneManager.GetSceneByName(GameSceneName);
        if (gameScene.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(GameSceneName);
            Debug.Log("PlayingState: Unloading GameScene after delay.");
        }
        else
        {
            yield return null;
            Debug.Log("PlayingState: GameScene is not loaded, nothing to unload.");
        }
    }
}
