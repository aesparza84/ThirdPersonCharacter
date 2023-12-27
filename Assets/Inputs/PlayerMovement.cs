using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody myBody;
    //private PlayerInputs input; //My custom action map
    private InputManager inputHandler;

    //private Vector3 moveVector;
    private Vector2 moveVector;
    [SerializeField] private float BaseSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float CurrentSpeed;

    private CinemachineFreeLook followCam;

    #region Initialization
    private void Awake()
    {
        //input = new PlayerInputs();
        inputHandler = GetComponent<InputManager>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody>();
        moveVector = Vector2.zero;

        followCam = GameObject.Find("PlayerFollowCam").GetComponent<CinemachineFreeLook>();

        SprintSpeed = BaseSpeed * 2.3f;
        CurrentSpeed = BaseSpeed;

        inputHandler.OnMovementPerf += MovementPerformed;
        inputHandler.OnMovementCanc += MovementCancelled;
        inputHandler.OnSprintPerf += SprintPerformed;
        inputHandler.OnSprintCanc += SprintCancelled;
    }

    private void SprintCancelled(object sender, System.EventArgs e)
    {
        CurrentSpeed = BaseSpeed;
    }

    private void SprintPerformed(object sender, System.EventArgs e)
    {
        CurrentSpeed = SprintSpeed;
    }

    private void MovementCancelled(object sender, System.EventArgs e)
    {
        moveVector = Vector2.zero;
    }

    private void MovementPerformed(object sender, Vector2 e)
    {
        moveVector = e;
    }
    #endregion

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        myBody.velocity = moveVector * CurrentSpeed;
    }

    #region Original Input
    //private void OnSprintCancelled(InputAction.CallbackContext obj)
    //{
    //    CurrentSpeed = BaseSpeed;
    //}

    //private void OnSprintPerformed(InputAction.CallbackContext obj)
    //{
    //    CurrentSpeed = SprintSpeed;
    //}
    //private void OnMovementPerformed(InputAction.CallbackContext obj)
    //{
    //    //moveVector = obj.ReadValue<Vector3>();

    //}

    //private void OnMovementCancelled(InputAction.CallbackContext obj) 
    //{
    //    moveVector = Vector2.zero;
    //}
    #endregion
}
