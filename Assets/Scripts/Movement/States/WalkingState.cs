using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MovingState
{
    public WalkingState(MoveStateManager context)
    {
        context.StartedSprint += OnSprint;
        context.StoppedWalking += OnStoppedWalking;
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
        if(active)
        {            
            e.switctStates(e.crouchWalkState);
        }
    }

    private void OnStoppedWalking(object sender, MoveStateManager e)
    {
        if(active) 
        {
            e.switctStates(e.idleState);

        }
    }

    private void OnSprint(object sender, MoveStateManager e)
    {
        if(active)
        {
            e.switctStates(e.runnningState);

        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        if (speed < context.BaseSpeed)
        {
            speed += Time.deltaTime * 5.0f;
            speed = Mathf.Clamp(speed, 0, context.BaseSpeed);
        }
        else if (speed > context.BaseSpeed)
        {
            speed -= Time.deltaTime * 5.0f;
            speed = Mathf.Clamp(speed, context.BaseSpeed, 10);
        }


        context.Currentspeed = speed;
        context.MyAnimator.SetFloat("Speed", context.Currentspeed);

        if (context.inputZeroCheck == 0)
        {
            context.switctStates(context.idleState);
        }
    }

    public override void EnterState(MoveStateManager context)
    {
        active = true;
        context.MyAnimator.SetBool("IsWalking", true);

        speed = context.Currentspeed;
    }

    public override void ExitState(MoveStateManager context)
    {
        active = false;
    }

    public override void DoFixedUpate(MoveStateManager context)
    {

    }
}
