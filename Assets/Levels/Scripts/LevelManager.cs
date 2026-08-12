using UnityEngine;
using UnityEngine.SceneManagement;
using Levels;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Current Level")]
    [SerializeField] private LevelData _currentLevel;

    public LevelData CurrentLevel => _currentLevel;

    [Header("Events")]
    [SerializeField] private GameEvent _onLevelLoaded;
    [SerializeField] private GameEvent _onLevelComplete;
    [SerializeField] private GameEvent _onLevelFailed;

    public GameEvent OnLevelLoaded => _onLevelLoaded;
    public GameEvent OnLevelComplete => _onLevelComplete;
    public GameEvent OnLevelFailed => _onLevelFailed;

    private const string GameSceneName = "GameScene";
    private bool _isLoadingGameScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGameScene()
    {
        LoadLevel(_currentLevel);
    }

    public void SetCurrentLevel(LevelData levelData)
    {
        _currentLevel = levelData;
    }

    public void LoadLevel(LevelData levelData)
    {
        if (levelData != null)
        {
            _currentLevel = levelData;
        }

        if (_currentLevel == null)
        {
            Debug.LogError("LevelManager: No LevelData assigned to load.");
            return;
        }

        if (_isLoadingGameScene)
        {
            Debug.Log("LevelManager: GameScene is already loading");
            return;
        }

        StartCoroutine(LoadLevelSceneAsyncRoutine());
    }

    public void LevelComplete()
    {
        Debug.Log("LevelManager: Level completed successfully!");
        _onLevelComplete?.Raise();
    }

    public void LevelFailed()
    {
        Debug.Log("LevelManager: Level failed!");
        _onLevelFailed?.Raise();
    }

    private System.Collections.IEnumerator LoadLevelSceneAsyncRoutine()
    {
        _isLoadingGameScene = true;
        string sceneToLoad = !string.IsNullOrWhiteSpace(_currentLevel?.sceneName)
            ? _currentLevel.sceneName
            : GameSceneName;

        Debug.Log($"LevelManager: Loading '{sceneToLoad}' asynchronously");

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        if (loadOperation == null)
        {
            Debug.LogError($"LevelManager: Failed to start async load for '{sceneToLoad}'");
            _isLoadingGameScene = false;
            yield break;
        }

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        _isLoadingGameScene = false;
        Debug.Log($"LevelManager: '{sceneToLoad}' loaded");
        _onLevelLoaded?.Raise();
    }
}
