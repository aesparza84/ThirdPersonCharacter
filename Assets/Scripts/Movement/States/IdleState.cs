using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovingState
{
    public IdleState(PlayerMovement passedPlayer, Animator passedAnim)
    {
        player = passedPlayer;
        playerBody = player.myBody;
        animator = passedAnim;
    }
    public override void DoUpdateAction()
    {
        //Nothing for idle
    }

    public override void EnterState()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
    }

    public override void ExitState()
    {

    }

    public override void SwitchToState()
    {

    }
}
