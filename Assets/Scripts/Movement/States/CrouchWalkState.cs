using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchWalkState : MovingState
{
    public CrouchWalkState(MoveStateManager context) 
    {
        context.StoppedWalking += OnStopWalk;
        context.StoppedCrouch += OnStopCrouch;
        context.StartedSprint += OnSprintFromCrouch;
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

    private void OnSprintFromCrouch(object sender, MoveStateManager e)
    {
        if (active)
        {
            leaveCrouch(e);
            e.switctStates(e.runnningState);
        }
    }

    private void OnStopCrouch(object sender, MoveStateManager e)
    {
        if (active)
        {
            //e.crouched = false;
            //e.MyAnimator.SetBool("IsCrouching", false);
            leaveCrouch(e);
            e.switctStates(e.walkingState);
        }
    }

    private void OnStopWalk(object sender, MoveStateManager e)
    {
        if (active)
        {
            e.MyAnimator.SetBool("IsWalking", false);
            e.switctStates(e.crouchState);
        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        context.Currentspeed = context.CrouchSpeed; 
    }

    public override void EnterState(MoveStateManager context)
    {
        active = true;
        context.crouched = true;
        context.ToggleColliders(false,true);
        context.MyAnimator.SetBool("IsWalking", true);
        context.MyAnimator.SetBool("IsCrouching", true); //We can get here from 'Walking'
    }

    public override void ExitState(MoveStateManager context)
    {
        active = false;
    }

    private void leaveCrouch(MoveStateManager context)
    {
        context.crouched = false;
        context.ToggleColliders(true, false);
        context.MyAnimator.SetBool("IsCrouching", false);
    }

    public override void DoFixedUpate(MoveStateManager context)
    {

    }
}
