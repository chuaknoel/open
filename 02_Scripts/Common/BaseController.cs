using Enums;
using System;
using System.Collections.Generic;

public abstract class BaseController<T> where T : BaseCreature
{
    public Dictionary<StateEnum, BaseState<T>> registedState = new Dictionary<StateEnum, BaseState<T>>();

    public BaseState<T> CurrentState => currentState;
    protected BaseState<T> currentState;
    protected BaseState<T> previousState;

    protected T owner;

    public BaseController(T owner)
    {
        this.owner = owner;
    }

    public void SetInitState(BaseState<T> initState)
    {
        RegisterState(initState);
        currentState = initState;
        currentState.OnEnter();
    }

    public virtual void OnUpdate(float deltaTime)
    {
        currentState.OnUpdate(deltaTime);
    }

    public virtual void OnFixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public virtual void RegisterState(BaseState<T> state)
    {
        state.Init(owner);
        registedState[state.GetState()] = state;
    }

    public virtual void ChangeState(StateEnum newState)
    {
        if (currentState.GetState().Equals(newState)) return;
       
        previousState = currentState;                       //원래 스테이트를 과거 스테이트로 변경
        currentState = registedState[newState];             //새로 들어온 스테이트를 현재 스테이트로 변경
        
        previousState?.OnExit();                            //과거 스테이트 종료

        currentState.OnEnter();                             //현재 스테이트 진입
    }

    public virtual void ResetState()
    {
        previousState = currentState;
        currentState = registedState[StateEnum.Idle];
        previousState?.OnExit();                           
        currentState.OnEnter();
    }

    public virtual BaseState<T> GetCurrentState()
    {
        return currentState;
    }

    public void OnDestroy()
    {
        foreach (var state in registedState)
        {
            state.Value.OnDestory();
        }
    }
}