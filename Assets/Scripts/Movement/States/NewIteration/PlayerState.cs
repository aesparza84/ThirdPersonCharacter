using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerMoveManager _context;
    protected PlayerMoveFactory _factory;
    protected PlayerState currentSubState;
    protected PlayerState currentParentState;

    protected bool parentState;

    protected float speed;
    public PlayerState(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory)
    {
        parentState = false;
        _context = passedContext;
        _factory = passedFactory;
    }
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void CheckSwitchConditions();
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void ChooseSubState();
    protected abstract void ToggleAnimationBool(bool toggle);
    protected void SwitchToState(PlayerState nextState)
    {
        ExitState();
        nextState.EnterState();
        if (!parentState)
        {
            _context.CurrentState.currentSubState = nextState;
            //currentParentState.SwitchSubState(nextState);
        }
        else 
        {         
            _context.CurrentState = nextState;
        }
    }

    private void SetParentedState(PlayerState nextParentState)
    {
        currentParentState = nextParentState;
    }
    protected void SwitchSubState(PlayerState nextSubState)
    {
        nextSubState.EnterState();
        currentSubState = nextSubState;
        currentSubState.SetParentedState(this);
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
