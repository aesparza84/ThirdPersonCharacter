using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverState : MovingState
{
    public CoverState(MoveStateManager context)
    {
        context.StoppedCover += OnLeaveCover;
    }

    private void OnLeaveCover(object sender, MoveStateManager e)
    {
        if (active)
        {
            Debug.Log("Left Cover");
            e.inCover = false;
            e.switctStates(e.idleState);
        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {

    }

    public override void EnterState(MoveStateManager context)
    {
        //We enter the 'covered' state
        active = true;
        context.inCover = true;
        context.Currentspeed = context.CoverSpeed;
    }

    public override void ExitState(MoveStateManager context)
    {

    }
}
