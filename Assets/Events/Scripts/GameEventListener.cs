using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent _event;
    [SerializeField] private UnityEvent _response;

    private void OnEnable()
    {
        if (_event == null)
        {
            Debug.LogWarning($"GameEventListener on '{gameObject.name}' has no GameEvent assigned.");
            return;
        }

        _event.RegisterListener(OnEventRaised);
    }

    private void OnDisable()
    {
        if (_event == null) return;

        _event.UnregisterListener(OnEventRaised);
    }

    public void OnEventRaised()
    {
        _response?.Invoke();
    }
}
