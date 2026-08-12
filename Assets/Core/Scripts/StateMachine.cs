using UnityEngine;
using System;

public class StateMachine
{
    private IState _currentState;

    public IState CurrentState => _currentState;

    public event Action<IState> OnStateChanged;

    public void Initialize(IState startingState)
    {
        _currentState = startingState;
        _currentState?.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        if (nextState == null)
        {
            Debug.LogError("Cannot transition to a null state!");
            return;
        }

        if (_currentState == nextState)
        {
            Debug.LogWarning("Already in state: " + nextState.GetType().Name);
            return;
        }

        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();

        OnStateChanged?.Invoke(_currentState);
    }

    public void Update()
    {
        _currentState?.Update();
    }
}
