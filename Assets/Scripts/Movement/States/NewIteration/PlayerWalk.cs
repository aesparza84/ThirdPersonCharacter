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
            SwitchSubState(_factory.Idle());
        }
        else if (_context.RunPressed)
        {
            SwitchSubState(_factory.Run());
        }
    }

    public override void ChooseSubState()
    {
        //No subsate for Walk
    }

    public override void EnterState()
    {
        Debug.Log("Now In Walk State");
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
