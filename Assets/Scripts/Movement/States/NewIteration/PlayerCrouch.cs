using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : PlayerState
{
    public PlayerCrouch(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory) : base(passedContext, passedFactory)
    {

    }

    public override void CheckSwitchConditions()
    {
        if (_context.RunPressed && _context.IsMoving)
        {
            SwitchToState(_factory.Run());
        }
        else if (_context.CrouchPressed && _context.IsMoving)
        {
            SwitchToState(_factory.Walk());
        }
        else if (_context.CrouchPressed)
        {
            SwitchToState(_factory.Idle());
        }
        else if (_context.CoverPressed && _context.CoverRayCast.LookForCover())
        {
            SwitchToState(_factory.Cover());
        }
        else if (_context.JumpedVaultPressed && _context.IsMoving && _context.CanVault())
        {
            SwitchToState(_factory.Vault());
        }
    }

    public override void ChooseSubState()
    {

    }

    public override void EnterState()
    {
        _context.CrouchPressed = false;
        _context.Currentspeed = _context.CrouchSpeed;

        ToggleAnimationBool(true);
    }

    public override void ExitState()
    {
        //_context.CrouchPressed = false;
        ToggleAnimationBool(false);
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        Debug.Log("Crouching");
        CheckSwitchConditions();
    }

    protected override void ToggleAnimationBool(bool toggle)
    {
        _context.MyAnimator.SetBool("IsCrouching", toggle);
    }
}
