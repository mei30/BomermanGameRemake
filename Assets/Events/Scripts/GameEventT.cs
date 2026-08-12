using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic ScriptableObject event that carries a payload of type T.
/// Cannot use [CreateAssetMenu] directly on a generic class — use a
/// concrete subclass instead (see examples below).
/// </summary>
public abstract class GameEvent<T> : ScriptableObject
{
    private readonly List<System.Action<T>> _listeners = new List<System.Action<T>>();

    public void Raise(T value)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i]?.Invoke(value);
        }
    }

    public void RegisterListener(System.Action<T> listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void UnregisterListener(System.Action<T> listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }
}
