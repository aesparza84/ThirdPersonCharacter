using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovingState
{
    public IdleState(MoveStateManager context)
    {

    }
    public override void DoUpdateAction(MoveStateManager context)
    {
        //Nothing for idle
    }

    public override void EnterState(MoveStateManager context)
    {
        context.MyAnimator.SetBool("IsWalking", false);
        context.MyAnimator.SetBool("IsRunning", false);
        context.Currentspeed = 0;

        //animator.SetBool("IsWalking", false);
        //animator.SetBool("IsRunning", false);
    }

    public override void ExitState(MoveStateManager context)
    {
        //Nothing for idle, this animation is the default
    }
}
