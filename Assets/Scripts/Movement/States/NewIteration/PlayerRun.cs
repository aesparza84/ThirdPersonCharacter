using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : PlayerState
{
    public PlayerRun(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory) : base(passedContext, passedFactory)
    {

    }

    public override void CheckSwitchConditions()
    {
        if (!_context.IsMoving)
        {
            SwitchToState(_factory.Idle());
        }
        else if (!_context.RunPressed)
        {
            SwitchToState(_factory.Walk());
        }
        else if (_context.CoverPressed && _context.CoverRayCast.LookForCover())
        {
            SwitchToState(_factory.Cover());
        }
    }

    public override void ChooseSubState()
    {
        //No substate for Run
    }

    public override void EnterState()
    {
        _context.Currentspeed = _context.SprintSpeed;
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
        Debug.Log("Running");
        CheckSwitchConditions();
    }

    protected override void ToggleAnimationBool(bool toggle)
    {
        _context.MyAnimator.SetBool("IsRunning", toggle);
    }
}
