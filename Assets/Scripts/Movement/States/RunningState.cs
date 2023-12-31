using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : MovingState
{
    public RunningState(MoveStateManager context)
    {
        context.StoppedSprint += OnSprintStop;
    }

    private void OnSprintStop(object sender, MoveStateManager e)
    {
        e.switctStates(e.walkingState);
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        //TODO: Import runnning rigidbody code
        context.Currentspeed = context.SprintSpeed;
    }

    public override void EnterState(MoveStateManager context)
    {
        context.MyAnimator.SetBool("IsRunning", true);
    }

    public override void ExitState(MoveStateManager context)
    {
        context.MyAnimator.SetBool("IsRunning", false);
    }
}
