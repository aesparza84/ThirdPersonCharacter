using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MovingState
{
    public WalkingState(MoveStateManager context)
    {
        context.StartedSprint += OnSprint;
        context.StoppedWalking += OnStoppedWalking;
        context.StartedCrouch += OnCrouch;
        UsesFixedUpdt = false;
    }

    private void OnCrouch(object sender, MoveStateManager e)
    {
        if(active)
        {
            e.switctStates(e.crouchWalkState);
        }
    }

    private void OnStoppedWalking(object sender, MoveStateManager e)
    {
        if(active) 
        {
            e.switctStates(e.idleState);

        }
    }

    private void OnSprint(object sender, MoveStateManager e)
    {
        if(active)
        {
            e.switctStates(e.runnningState);

        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        context.Currentspeed = context.BaseSpeed;
        if (context.inputZeroCheck == 0)
        {
            context.switctStates(context.idleState);
        }
    }

    public override void EnterState(MoveStateManager context)
    {
        active = true;
        context.MyAnimator.SetBool("IsWalking", true);
    }

    public override void ExitState(MoveStateManager context)
    {
        active = false;
    }

    public override void DoFixedUpate(MoveStateManager context)
    {
        throw new System.NotImplementedException();
    }
}
