using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : PlayerState
{
    public PlayerFall(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory) : base(passedContext, passedFactory)
    {
        parentState = true;
    }

    public override void CheckSwitchConditions()
    {
        if (_context.IsGrounded)
        {
            SwitchToState(_factory.Grounded());
        }
    }

    public override void ChooseSubState()
    {
        //No Substates for Fall
    }

    public override void EnterState()
    {
        Debug.Log("Now in Fall state");
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdate()
    {
        if (currentSubState != null)
        {
            currentSubState.FixedUpdate();
        }
        applyGravity();
    }

    public override void Update()
    {        
        if(currentSubState != null)
        {
            currentSubState.Update();
        }
        CheckSwitchConditions();
    }

    private void applyGravity()
    {
        _context.PlayerBody.velocity += Vector3.down * _context.GravityMultiplier;
    }
}