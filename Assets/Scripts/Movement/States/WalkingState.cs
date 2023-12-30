using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MovingState
{
    public WalkingState(PlayerMovement passedPlayer, Animator passedAnim)
    {
        player = passedPlayer;
        playerBody = player.myBody;
        animator = passedAnim;
    }
    public override void DoUpdateAction()
    {

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
