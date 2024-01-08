using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CrouchState : MovingState
{
    public CrouchState(MoveStateManager context)
    {
        context.StoppedCrouch += OnStoppedCrouch;
        context.StartedWalking += OnStartWalk;
        context.StartedCover += OnConver;
        UsesFixedUpdt = false;
    }

    private void OnConver(object sender, MoveStateManager e)
    {
        if (active && e.coverRayCast.LookForCover())
        {
            //e.PlayerBody.MovePosition(e.coverRayCast.CoverPoint);
            e.switctStates(e.coverState);
        }
    }

    private void OnStartWalk(object sender, MoveStateManager e)
    {
        if (active)
        {
            e.switctStates(e.crouchWalkState);
        }
    }

    private void OnStoppedCrouch(object sender, MoveStateManager e)
    {
        if (active)
        {
            e.crouched = false;
            e.ToggleColliders(true, false);
            e.MyAnimator.SetBool("IsCrouching", false);
            e.switctStates(e.idleState);
        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        if (speed > 0)
        {
            context.inputZeroCheck = 0;
            speed -= Time.deltaTime * 4.0f;
        }
        else if (speed < 0.0f)
        {
            speed = 0.0f;
            inputX = 0;
            inputY = 0;
        }
        context.MyAnimator.SetFloat("Speed", speed);

        context.HorizontalInput = inputX;
        context.VerticalInput = inputY;
        context.Currentspeed = speed;        
    }

    public override void EnterState(MoveStateManager context)
    {
        active = true;
        context.crouched = true;
        speed = context.Currentspeed;

        inputX = context.HorizontalInput;
        inputY = context.VerticalInput;

        context.MyAnimator.SetBool("IsCrouching", true);
        context.ToggleColliders(false, true);
    }

    public override void ExitState(MoveStateManager context)
    {
        context.ToggleColliders(true, false);
        active = false;
    }

    public override void DoFixedUpate(MoveStateManager context)
    {

    }
}
