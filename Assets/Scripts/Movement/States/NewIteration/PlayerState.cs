using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerMoveManager _context;
    protected PlayerMoveFactory _factory;
    protected PlayerState currentSubState;
    public PlayerState(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory)
    {
        _context = passedContext;
        _factory = passedFactory;
    }
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void CheckSwitchConditions();
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void ChooseSubState();
    protected void SwitchToState(PlayerState nextState)
    {
        ExitState();
        nextState.EnterState();
        _context.CurrentState = nextState;
    }

    protected void SwitchSubState(PlayerState nextSubState)
    {
        nextSubState.EnterState();
        currentSubState = nextSubState;
    }

    protected void UpdateSubState()
    {
        currentSubState.Update();
    }

    protected void FixedUpdateSubState()
    {
        currentSubState.FixedUpdate();  
    }
}
