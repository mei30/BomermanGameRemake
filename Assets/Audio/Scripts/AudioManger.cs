using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _levelMusic;
    [SerializeField] private AudioClip _playerDeathClip;
    [SerializeField] private GameEvent _onLevelLoaded;
    [SerializeField] private GameEvent _onLevelCompleted;
    [SerializeField] private GameEvent _onPlayerDied;

    public AudioSource AudioSource => _audioSource;
    public GameEvent OnLevelLoaded => _onLevelLoaded;
    public GameEvent OnLevelCompleted => _onLevelCompleted;
    public GameEvent OnPlayerDied => _onPlayerDied;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _onLevelLoaded?.RegisterListener(HandleOnLevelLoaded);
        _onLevelCompleted?.RegisterListener(StopLevelMusic);
        _onPlayerDied?.RegisterListener(PlayDeathClip);
    }

    private void OnDisable()
    {
        _onLevelLoaded?.UnregisterListener(HandleOnLevelLoaded);
        _onLevelCompleted?.UnregisterListener(StopLevelMusic);
        _onPlayerDied?.UnregisterListener(PlayDeathClip);
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"AudioManager: Level loaded -> {scene.name}");
        _onLevelLoaded?.Raise();
    }

    private void HandleOnLevelLoaded()
    {
        if (_audioSource == null)
        {
            Debug.LogWarning("AudioManager: Missing AudioSource.");
            return;
        }

        if (_levelMusic == null)
        {
            Debug.LogWarning("AudioManager: No level music clip assigned.");
            return;
        }

        _audioSource.clip = _levelMusic;
        _audioSource.loop = true;
        _audioSource.Play();

        Debug.Log("AudioManager: Playing level music from OnLevelLoaded event.");
    }

    private void PlayDeathClip()
    {
        // if (_audioSource == null)
        // {
        //     Debug.LogWarning("AudioManager: Missing AudioSource.");
        //     return;
        // }

        // if (_playerDeathClip == null)
        // {
        //     Debug.LogWarning("AudioManager: No player death clip assigned.");
        //     return;
        // }

        // _audioSource.PlayOneShot(_playerDeathClip);
        // Debug.Log("AudioManager: Playing player death clip.");
    }

    private void StopLevelMusic()
    {
        if (_audioSource == null)
        {
            Debug.LogWarning("AudioManager: Missing AudioSource.");
            return;
        }

        _audioSource.Stop();
        Debug.Log("AudioManager: Stopped level music from OnLevelCompleted event.");
    }
}
