using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : MovingState
{
    public RunningState(MoveStateManager context)
    {
        managerContext = context;

        context.StoppedSprint += OnSprintStop;
        context.StartedCover += OnCover;
        UsesFixedUpdt = false;
    }

    private void OnCover(object sender, MoveStateManager e)
    {
        if (active && e.coverRayCast.LookForCover())
        {
            //e.PlayerBody.MovePosition(e.coverRayCast.CoverPoint);
            e.switctStates(e.coverState);
        }
    }

    private void OnSprintStop(object sender, MoveStateManager e)
    {
        if (active)
        {
            e.switctStates(e.walkingState);
        }
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

    public override void DoFixedUpate(MoveStateManager context)
    {

    }
}
