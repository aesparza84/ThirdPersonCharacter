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
    }

    public override void ChooseSubState()
    {
        //No subsate for Walk
    }

    public override void EnterState()
    {
        _context.Currentspeed = _context.BaseSpeed;
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        Debug.Log("Walking");
        CheckSwitchConditions();
    }
}
