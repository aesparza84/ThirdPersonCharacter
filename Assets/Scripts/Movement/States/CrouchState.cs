using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : MovingState
{
    public CrouchState(PlayerMovement passedPlayer, Animator passedAnim)
    {
        player = passedPlayer;
        playerBody = player.myBody;
        animator = passedAnim;
    }
    public override void DoUpdateAction()
    {
        throw new System.NotImplementedException();
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void SwitchToState()
    {
        throw new System.NotImplementedException();
    }
}
