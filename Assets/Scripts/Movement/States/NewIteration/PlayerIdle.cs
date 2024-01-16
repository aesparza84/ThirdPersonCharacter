using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory) : base(passedContext, passedFactory)
    {

    }

    public override void CheckSwitchConditions()
    {
        if (_context.IsMoving)
        {
            SwitchToState(_factory.Walk());
        }
        else if (_context.CrouchPressed)
        {
            SwitchToState(_factory.Crouch());
        }
        else if (_context.CoverPressed && _context.CoverRayCast.LookForCover())
        {
            SwitchToState(_factory.Cover());
        }
    }

    public override void ChooseSubState()
    {
        //No substate for Idle
    }

    public override void EnterState()
    {
        ToggleAnimationBool(false);
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        Debug.Log("Idling");
        CheckSwitchConditions();
    }

    protected override void ToggleAnimationBool(bool toggle)
    {
        _context.MyAnimator.SetBool("IsWalking", toggle);
        _context.MyAnimator.SetBool("IsRunning", toggle);
    }
}
