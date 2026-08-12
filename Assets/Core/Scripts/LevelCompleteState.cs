using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteState : IState
{
    private const string LevelCompleteSceneName = "LevelComplete";

    public void Enter()
    {
        Debug.Log("Entering Level Complete State");

        if (GameManager.Instance == null)
        {
            Debug.LogError("LevelCompleteState: GameManager instance is missing, cannot load LevelComplete scene.");
            return;
        }

        GameManager.Instance.StartCoroutine(LoadLevelCompleteSceneAsync());
    }

    public void Exit()
    {
        Debug.Log("Exiting Level Complete State");

        if (GameManager.Instance == null)
        {
            Debug.LogError("LevelCompleteState: GameManager instance is missing, cannot unload LevelComplete scene.");
            return;
        }

        GameManager.Instance.StartCoroutine(UnloadLevelCompleteSceneAsync());
    }

    public void Update()
    {
        // Level Complete update logic here
    }

    private IEnumerator LoadLevelCompleteSceneAsync()
    {
        Scene levelCompleteScene = SceneManager.GetSceneByName(LevelCompleteSceneName);
        if (levelCompleteScene.isLoaded)
        {
            Debug.Log("LevelComplete scene is already loaded");
            yield break;
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(LevelCompleteSceneName, LoadSceneMode.Additive);
        if (loadOperation == null)
        {
            Debug.LogError("LevelCompleteState: Failed to start async load for LevelComplete scene.");
            yield break;
        }

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        Debug.Log("Loaded LevelComplete scene asynchronously");
    }

    private IEnumerator UnloadLevelCompleteSceneAsync()
    {
        Scene levelCompleteScene = SceneManager.GetSceneByName(LevelCompleteSceneName);
        if (!levelCompleteScene.isLoaded)
        {
            Debug.Log("LevelComplete scene is not loaded, nothing to unload");
            yield break;
        }

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(LevelCompleteSceneName);
        if (unloadOperation == null)
        {
            Debug.LogError("LevelCompleteState: Failed to start async unload for LevelComplete scene.");
            yield break;
        }

        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        Debug.Log("Unloaded LevelComplete scene asynchronously");
    }
}
