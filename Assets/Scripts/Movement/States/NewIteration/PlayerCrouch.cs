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
            _context.CrouchedCover = true;
            SwitchToState(_factory.Cover());
        }
        else if (_context.JumpedVaultPressed && _context.IsMoving && _context.CanVault())
        {
            SwitchToState(_factory.Vault());
        }
        else if (_context.IsMoving && _context.JumpedVaultPressed && _context.CanVault())
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
        _context.Crouched = true;
        _context.ToggleColliders(!_context.Crouched, _context.Crouched);
        speed = _context.Currentspeed;

        ToggleAnimationBool(true);
    }

    public override void ExitState()
    {
        _context.CrouchPressed = false;
        _context.Crouched = false;
        ToggleAnimationBool(false);
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        Debug.Log("Crouching");
        CheckSwitchConditions();

        if (_context.IsMoving && speed < _context.CrouchSpeed)
        {
            speed += Time.deltaTime * 10.0f;
            speed = Mathf.Clamp(speed, 0, _context.CrouchSpeed);
        }
        else if (speed > _context.CrouchSpeed)
        {
            speed -= Time.deltaTime * 10.0f;
            speed = Mathf.Clamp(speed, _context.CrouchSpeed, 10);
        }
        else if (!_context.IsMoving)
        {
            speed -= Time.deltaTime * 10.0f;
            speed = Mathf.Clamp(speed, 0, 10);
        }

        _context.Currentspeed = speed;
        _context.MyAnimator.SetFloat("Speed", speed);
    }

    protected override void ToggleAnimationBool(bool toggle)
    {
        _context.MyAnimator.SetBool("IsCrouching", toggle);
    }
}
