using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerState
{
    public PlayerGrounded(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory) : base(passedContext, passedFactory)
    {
        ChooseSubState();
    }

    public override void CheckSwitchConditions()
    {
        if (!_context.IsGrounded)
        {
            SwitchToState(_factory.Fall());
        }
    }

    public override void ChooseSubState()
    {
        if (_context.IsMoving)
        {
            SwitchSubState(_factory.Walk());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Now In Grounded State");
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdate()
    {
        Debug.Log("You should always see this: Grounded");
        if (currentSubState != null)
        {
            currentSubState.FixedUpdate();
        }
    }

    public override void Update()
    {
        if (currentSubState != null)
        {
            currentSubState.Update();
        }
        CheckSwitchConditions();
        ChooseSubState();
    }
}
