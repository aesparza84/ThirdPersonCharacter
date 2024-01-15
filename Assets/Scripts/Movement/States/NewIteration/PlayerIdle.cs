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
            SwitchSubState(_factory.Walk());
        }
    }

    public override void ChooseSubState()
    {
        //No substate for Idle
    }

    public override void EnterState()
    {
        Debug.Log("Now In Idle State");
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
