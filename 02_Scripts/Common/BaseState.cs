using System;
using Enums;

public abstract class BaseState<T> where T : BaseCreature
{
    public BaseState() { }

    protected float elapsedTime;

    protected T owner;
    protected abstract StateEnum stateType { get; }

    public virtual void Init(T owner) 
    {
        this.owner = owner;
        elapsedTime = 0;
    }

    public virtual void OnEnter() 
    {
        //Logger.Log(this.GetType().Name);
        elapsedTime = 0;
    }

    public virtual void OnUpdate(float deltaTime)
    {
        elapsedTime += deltaTime;
    }
    public virtual void OnFixedUpdate() { }

    public virtual void OnExit() 
    {
        elapsedTime = 0;
    }

    public virtual void OnDestory() { }

    public StateEnum GetState()
    {
        return stateType;
    }
}
