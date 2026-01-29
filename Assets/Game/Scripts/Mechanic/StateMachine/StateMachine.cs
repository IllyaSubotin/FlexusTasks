using System;
using System.Collections.Generic;

public class StateMachine
{
    private readonly Dictionary<Type, State> _states;
    private State _currentState;

    public StateMachine(IEnumerable<State> states)
    {
        _states = new Dictionary<Type, State>();

        foreach (var state in states)
        {
            _states[state.GetType()] = state;
        }
    }

    public void ChangeState<T>() where T : State
    {
        var newState = _states[typeof(T)];
        ChangeState(newState);
    }

    private void ChangeState(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
