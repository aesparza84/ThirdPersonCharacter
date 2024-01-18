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
            SwitchToState(_factory.Idle());
        }
        else if (_context.RunPressed)
        {
            SwitchToState(_factory.Run());
        }
        else if (_context.CrouchPressed)
        {
            SwitchToState(_factory.Crouch());
        }
        else if (_context.CoverPressed && _context.CoverRayCast.LookForCover())
        {
            SwitchToState(_factory.Cover());
        }
        else if (_context.JumpedVaultPressed && _context.CanVault())
        {
            SwitchToState(_factory.Vault());
        }
    }

    public override void ChooseSubState()
    {
        //No subsate for Walk
    }

    public override void EnterState()
    {
        speed = _context.Currentspeed;
        //_context.Currentspeed = _context.BaseSpeed;
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
        CheckSwitchConditions();

        if (speed < _context.BaseSpeed)
        {
            speed += Time.deltaTime * 10.0f;
            speed = Mathf.Clamp(speed, 0, _context.BaseSpeed);
        }
        else if (speed > _context.BaseSpeed)
        {
            speed -= Time.deltaTime * 10.0f;
            speed = Mathf.Clamp(speed, _context.BaseSpeed, 10);
        }

        //speed = _context.BaseSpeed;

        _context.Currentspeed = speed;
        _context.MyAnimator.SetFloat("Speed", _context.Currentspeed);
    }

    protected override void ToggleAnimationBool(bool toggle)
    {
        _context.MyAnimator.SetBool("IsWalking", toggle);

    }
}
