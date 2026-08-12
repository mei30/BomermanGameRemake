using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : IState
{
    private const string MainMenuSceneName = "MainMenu";

    public void Enter()
    {
        Debug.Log("Entering Main Menu State");

        Scene mainMenuScene = SceneManager.GetSceneByName(MainMenuSceneName);
        if (!mainMenuScene.isLoaded)
        {
            SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Additive);
            Debug.Log("Loaded MainMenu scene");
        }
        else
        {
            Debug.Log("MainMenu scene is already loaded");
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Main Menu State");

        Scene mainMenuScene = SceneManager.GetSceneByName(MainMenuSceneName);
        if (mainMenuScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(MainMenuSceneName);
            Debug.Log("Unloading MainMenu scene");
        }
        else
        {
            Debug.Log("MainMenu scene is not loaded, nothing to unload");
        }
    }

    public void Update()
    {
        // Main Menu update logic here
    }
}
