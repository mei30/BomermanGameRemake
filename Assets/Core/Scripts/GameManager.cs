using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Channels")]
    [SerializeField] private GameEvent _onLevelCompleted;
    [SerializeField] private GameEvent _onLevelFailed;

    private StateMachine _stateMachine;

    public StateMachine StateMachine => _stateMachine;

    // State instances
    private MainMenuState _mainMenuState;
    private PlayingState _playingState;
    private GameOverState _gameOverState;
    private LevelCompleteState _levelCompleteState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeStateMachine();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _onLevelCompleted?.RegisterListener(HandleLevelCompleted);
        _onLevelFailed?.RegisterListener(HandleLevelFailed);
    }

    private void OnDisable()
    {
        _onLevelCompleted?.UnregisterListener(HandleLevelCompleted);
        _onLevelFailed?.UnregisterListener(HandleLevelFailed);
    }

    private void InitializeStateMachine()
    {
        // Create state instances
        _mainMenuState = new MainMenuState();
        _playingState = new PlayingState();
        _gameOverState = new GameOverState();
        _levelCompleteState = new LevelCompleteState();

        // Initialize state machine with main menu as the starting state
        _stateMachine = new StateMachine();
        _stateMachine.Initialize(_mainMenuState);
    }

    private void Update()
    {
        _stateMachine?.Update();
    }

    private void HandleLevelCompleted()
    {
        GoToLevelCompleteState();
    }

    private void HandleLevelFailed()
    {
        GoToLevelCompleteState();
    }

    private void GoToLevelCompleteState()
    {
        if (_stateMachine == null || _levelCompleteState == null)
        {
            Debug.LogError("GameManager: Cannot transition to LevelComplete state.");
            return;
        }

        _stateMachine.TransitionTo(_levelCompleteState);
    }

    // Public methods for state transitions
    public void GoToMainMenu()
    {
        _stateMachine.TransitionTo(_mainMenuState);
    }

    public void StartGame()
    {
        _stateMachine.TransitionTo(_playingState);
    }

    public void GameOver()
    {
        _stateMachine.TransitionTo(_gameOverState);
    }
}
