using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerState
{
    public PlayerWalk(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory) : base(passedContext, passedFactory)
    {

    }

    public override void CheckSwitchConditions()
    {
        if (!_context.IsMoving)
        {
            SwitchToState(_factory.Idle());
        }
        else if (_context.RunPressed)
        {
            SwitchToState(_factory.Run());
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
        //No subsate for Walk
    }

    public override void EnterState()
    {
        _context.Currentspeed = _context.BaseSpeed;
        ToggleAnimationBool(true);
    }

    public override void ExitState()
    {
        ToggleAnimationBool(false);
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        Debug.Log("Walking");
        CheckSwitchConditions();
    }

    protected override void ToggleAnimationBool(bool toggle)
    {
        _context.MyAnimator.SetBool("IsWalking", toggle);

    }
}
