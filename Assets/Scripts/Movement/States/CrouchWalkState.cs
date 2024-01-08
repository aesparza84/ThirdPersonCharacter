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
        if (speed < context.CrouchSpeed)
        {
            speed += Time.deltaTime * 2.0f;
            speed = Mathf.Clamp(speed, 0, context.CrouchSpeed);
        }
        else if (speed > context.CrouchSpeed)
        {
            speed -= Time.deltaTime * 2.0f;
            speed = Mathf.Clamp(speed, context.CrouchSpeed, 10);
        }

        context.Currentspeed = speed;
        context.MyAnimator.SetFloat("Speed", speed);
    }

    public override void EnterState(MoveStateManager context)
    {
        active = true;
        context.crouched = true;
        speed = context.Currentspeed;
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
