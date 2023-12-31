using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MovingState
{
    public WalkingState(MoveStateManager context)
    {
        context.StartedSprint += OnSprint;
        context.StoppedWalking += OnStoppedWalking;
    }

    private void OnStoppedWalking(object sender, MoveStateManager e)
    {
        e.switctStates(e.idleState);
    }

    private void OnSprint(object sender, MoveStateManager e)
    {
        e.switctStates(e.runnningState);
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        //TODO: Walking rigidbody
        context.Currentspeed = context.BaseSpeed;        
    }

    public override void EnterState(MoveStateManager context)
    {
        context.MyAnimator.SetBool("IsWalking", true);
    }

    public override void ExitState(MoveStateManager context)
    {
        context.MyAnimator.SetBool("IsWalking", false);
    }
}
