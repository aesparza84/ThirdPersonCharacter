using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovingState
{
    public IdleState(MoveStateManager context)
    {
        context.StartedWalking += OnWalk;
        context.StartedCrouch += OnCrouch;
        context.StartedCover += OnCover;
    }

    private void OnCover(object sender, MoveStateManager e)
    {
        if (active && e.coverRayCast.LookForCover())
        {
            e.PlayerBody.MovePosition(e.coverRayCast.CoverPoint);
            e.switctStates(e.coverState);
        }
    }

    private void OnCrouch(object sender, MoveStateManager e)
    {
        if (active)
        {
            e.switctStates(e.crouchState);
        }
    }

    private void OnWalk(object sender, MoveStateManager e)
    {
        if (active)
        {
            e.switctStates(e.walkingState);
        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        //Nothing for idle
    }

    public override void EnterState(MoveStateManager context)
    {
        active = true;
        context.Currentspeed = 0;

        context.MyAnimator.SetBool("IsWalking", false);
        context.MyAnimator.SetBool("IsRunning", false);
        context.MyAnimator.SetBool("IsCrouching", false);

    }

    public override void ExitState(MoveStateManager context)
    {
        active = false;
        //Nothing for idle, this animation is the default
    }
}
