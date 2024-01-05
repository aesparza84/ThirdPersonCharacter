using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void DoFixedUpate(MoveStateManager context)
    {
        throw new System.NotImplementedException();
    }
}
