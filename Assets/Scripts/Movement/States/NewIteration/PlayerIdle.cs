using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

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
            //This checks if we should be lowered when going into cover
            if (_context.CanVault()) 
            {
                _context.CrouchedCover = true;
            }
            SwitchToState(_factory.Cover());
        }
    }

    public override void ChooseSubState()
    {
        //No substate for Idle
    }

    public override void EnterState()
    {
        _context.ToggleColliders(true, false);
        speed = _context.Currentspeed;
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
        CheckSwitchConditions();

        if (speed > 0)
        {            
            speed -= Time.deltaTime * 5.0f;
        }
        else if (speed < 0.0f)
        {
            speed = 0.0f;
        }

        _context.Currentspeed = speed;

        _context.MyAnimator.SetFloat("Speed", speed);
    }

    protected override void ToggleAnimationBool(bool toggle)
    {
        _context.MyAnimator.SetBool("IsWalking", toggle);
        _context.MyAnimator.SetBool("IsRunning", toggle);
    }
}
