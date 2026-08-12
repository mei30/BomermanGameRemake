using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void OnStartGamePressed()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is missing.");
            return;
        }

        Debug.Log("MainMenu: Start Game pressed");
        GameManager.Instance.StartGame();
    }

    public void OnQuitPressed()
    {
        Debug.Log("MainMenu: Quit pressed");
        Application.Quit();
    }
}