using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    ///This will handle all inputs for the Player:
    /// Movements, jump, crouch, cover, etc.
    /// 

    public PlayerInputs input;

    //public event EventHandler<Vector2> OnMovementPerf;
    //public event EventHandler OnMovementCanc;
    //public event EventHandler OnSprintPerf;
    //public event EventHandler OnSprintCanc;

    //public event EventHandler n;

    private Vector2 MoveVectorValue;

    private void OnEnable()
    {
        if (input == null)
        {
            input = new PlayerInputs();
        }

        input.Enable();

        //Custom Events
        //input.Player.MovementWASD.performed += OnMovementPerformed;
        //input.Player.MovementWASD.canceled += OnMovementCancelled;

        //input.Player.Sprint.performed += OnSprintPerformed;
        //input.Player.Sprint.canceled += OnSprintCancelled;
    }

    #region Evoking my custom events
    //public void OnSprintCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    //{
    //    OnSprintCanc.Invoke(this, EventArgs.Empty);
    //}

    //public void OnSprintPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    //{        
    //    OnSprintPerf.Invoke(this, EventArgs.Empty);
    //}

    //public void OnMovementCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    //{
    //    OnMovementCanc.Invoke(this, EventArgs.Empty);
    //}

    //public void OnMovementPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    //{
    //    MoveVectorValue = obj.ReadValue<Vector2>();
    //    OnMovementPerf.Invoke(this, MoveVectorValue);
    //}
    #endregion

    private void Update()
    {
        //Debug.Log("Movement vector: "+MoveVectorValue);
    }
    private void OnDisable()
    {
        input.Disable();
        //input.Player.MovementWASD.performed -= OnMovementPerformed;
        //input.Player.MovementWASD.canceled -= OnMovementCancelled;
        //input.Player.Sprint.performed -= OnSprintPerformed;
        //input.Player.Sprint.performed -= OnSprintCancelled;
    }

}
