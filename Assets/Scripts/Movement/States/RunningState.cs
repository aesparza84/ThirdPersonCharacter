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
        context.Currentspeed = context.SprintSpeed;
        if (context.inputZeroCheck == 0)
        {
            context.switctStates(context.idleState);
        }
    }

    public override void EnterState(MoveStateManager context)
    {
        active = true;
        context.MyAnimator.SetBool("IsRunning", true);
    }

    public override void ExitState(MoveStateManager context)
    {
        active = false;
        context.MyAnimator.SetBool("IsRunning", false);
    }
}
