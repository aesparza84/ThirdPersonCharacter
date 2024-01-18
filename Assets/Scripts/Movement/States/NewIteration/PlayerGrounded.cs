using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerState
{
    public PlayerGrounded(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory) : base(passedContext, passedFactory)
    {
        parentState = true;

        //This is called whenever we come BACK into the grounded state from the Fall state
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
        if (_context.IsMoving && _context.RunPressed)
        {
            SwitchSubState(_factory.Run());
        }
        else if (_context.IsMoving)
        {
            SwitchSubState(_factory.Walk());
        }
        else
        {
            SwitchSubState(_factory.Idle());
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

        //In grounded state we're always checking and moving with inputs
        setMoveVector();

        if (currentSubState != null)
        {
            currentSubState.FixedUpdate(); //This will override MoveVector if we decide to, in here
        }

        applyVelocity();
    }

    public override void Update()
    {
        SetPlayersForward();

        if (currentSubState != null)
        {
            currentSubState.Update();
        }


        CheckSwitchConditions();

        //Debug.Log("Current SUbstate-- "+currentSubState);
    }

    protected override void ToggleAnimationBool(bool toggle)
    {

    }

    private void setMoveVector()
    {
        _context.MoveVector = (_context.CamController.ForwardRotation.right * _context.HorizontalIput) +
                              (_context.CamController.ForwardRotation.forward * _context.VerticalIput);
    }
    private void applyVelocity()
    {
        _context.PlayerBody.velocity = _context.MoveVector.normalized * _context.Currentspeed;
    }

    private void SetPlayersForward()
    {
        //direction = followCam.forwaredRotation;
        if (_context.AimMode)
        {
            //This faces player to camera forward, AIM CAMERA
            //playerTransform.forward = direction.forward;
            _context.PlayerTransform.forward = _context.CamForward;
        }
        else
        {
            if (_context.MoveVector != Vector3.zero)
            {
                _context.PlayerTransform.forward = Vector3.Slerp(_context.PlayerTransform.forward,
                    new Vector3(_context.MoveVector.x, 0, _context.MoveVector.z).normalized,
                    Time.deltaTime * 10);
            }
        }
    }
}
