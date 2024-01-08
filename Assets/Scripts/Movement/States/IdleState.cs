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
        if (speed > 0)
        {
            context.inputZeroCheck = 0;
            speed -= Time.deltaTime * 5.0f;
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
        speed = context.Currentspeed;
        context.ToggleColliders(true, false);

        inputX = context.HorizontalInput;
        inputY = context.VerticalInput;

        context.MyAnimator.SetBool("IsWalking", false);
        context.MyAnimator.SetBool("IsRunning", false);
        context.MyAnimator.SetBool("IsCrouching", false);
    }

    public override void ExitState(MoveStateManager context)
    {
        active = false;
        //Nothing for idle, this animation is the default
    }

    public override void DoFixedUpate(MoveStateManager context)
    {

    }
}
