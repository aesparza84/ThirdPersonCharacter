using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : MovingState
{
    public RunningState(MoveStateManager context)
    {
        managerContext = context;

        context.StoppedSprint += OnSprintStop;
        context.StoppedWalking += OnNoInput;
        context.StartedCover += OnCover;
        UsesFixedUpdt = false;
    }

    private void OnNoInput(object sender, MoveStateManager e)
    {
        if (active)
        {
            e.switctStates(e.idleState);
        }
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
        if (speed < context.SprintSpeed)
        {
            speed += Time.deltaTime * 5.0f;
        }
        else if (speed > context.SprintSpeed)
        {
            speed = context.SprintSpeed;
        }

        context.Currentspeed = speed;
        context.MyAnimator.SetFloat("Speed", speed);
    }

    public override void EnterState(MoveStateManager context)
    {
        active = true;
        speed = context.Currentspeed;
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
