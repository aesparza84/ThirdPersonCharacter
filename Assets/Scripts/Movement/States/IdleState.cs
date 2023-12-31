using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovingState
{
    public IdleState(MoveStateManager context)
    {
        context.StartedWalking += OnWalk;
    }

    private void OnWalk(object sender, MoveStateManager e)
    {
        e.switctStates(e.walkingState);
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        //Nothing for idle
    }

    public override void EnterState(MoveStateManager context)
    {
        context.Currentspeed = 0;

        //animator.SetBool("IsWalking", false);
        //animator.SetBool("IsRunning", false);
    }

    public override void ExitState(MoveStateManager context)
    {
        //Nothing for idle, this animation is the default
    }
}
