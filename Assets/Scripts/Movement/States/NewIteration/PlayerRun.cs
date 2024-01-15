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
            SwitchSubState(_factory.Idle());
        }
        else if (!_context.RunPressed)
        {
            SwitchSubState(_factory.Walk());
        }
    }

    public override void ChooseSubState()
    {
        //No substate for Run
    }

    public override void EnterState()
    {
        Debug.Log("Now In Run State");
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        Debug.Log("Running");
        CheckSwitchConditions();
    }
}
