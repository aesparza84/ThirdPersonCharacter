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
    }

    public override void ChooseSubState()
    {
        //No substate for Idle
    }

    public override void EnterState()
    {

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
}
