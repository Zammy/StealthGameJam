using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ISMState
{
    void SetData(SMStateData data);
    void OnStateEnter();
    IEnumerator OnStateExecute(StateMachine stateMachine);
    void OnStateExit();
}

public interface IOverrideState : ISMState
{
    bool OverrideCurrentState(ISMState currentState);
}

public abstract class SMStateData : ScriptableObject
{
    public abstract Type GetStateType();
}

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    SMStateData[] StateData;

    Dictionary<Type, ISMState> _states;
    ISMState _currentState;
    Coroutine _currentStateExecute;

    void Awake()
    {
        _states = new Dictionary<Type, ISMState>();
    }

    void Update()
    {
        foreach (var kvp in _states)
        {
            var overrideState = kvp.Value as IOverrideState;
            if (overrideState != null 
                && _currentState != overrideState 
                && overrideState.OverrideCurrentState(_currentState))
            {
                ChangeState(overrideState.GetType());
            }
        }
    }

    public void AddState(ISMState state)
    {
        _states.Add(state.GetType(), state);
        var stateData = StateData.FirstOrDefault(s => s.GetStateType() == state.GetType());
        state.SetData(stateData);
    }

    public void ChangeState(Type stateType)
    {
        if (_currentState != null)
        {
            Debug.LogFormat("Exit [{0}]", _currentState.GetType().Name);
            _currentState.OnStateExit();
            StopCoroutine(_currentStateExecute);
        }
        _currentState = _states[stateType];
        Debug.LogFormat("Enter [{0}]", _currentState.GetType().Name);
        _currentState.OnStateEnter();
        Debug.LogFormat("Execute [{0}]", _currentState.GetType().Name);
        _currentStateExecute = StartCoroutine(_currentState.OnStateExecute(this));
    }
}