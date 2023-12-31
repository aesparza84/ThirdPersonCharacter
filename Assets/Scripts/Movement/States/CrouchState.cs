using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : MovingState
{
    public CrouchState(MoveStateManager context)
    {
        context.StoppedCrouch += OnStoppedCrouch;
        context.StartedWalking += OnStartWalk;
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
            e.MyAnimator.SetBool("IsCrouching", false);
            e.switctStates(e.idleState);
        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {

    }

    public override void EnterState(MoveStateManager context)
    {
        active = true;
        context.crouched = true;
        context.MyAnimator.SetBool("IsCrouching", true);
    }

    public override void ExitState(MoveStateManager context)
    {
        active = false;
    }
}
