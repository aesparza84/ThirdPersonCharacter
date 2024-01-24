using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("The Players Cameras")]
    [SerializeField] private CinemachineVirtualCameraBase thirdPersonCam;
    [SerializeField] private CinemachineVirtualCameraBase aimCamera;
    //[SerializeField] private CinemachineVirtualCameraBase AimLeftCam;
    private Camera mainCam;

    private CinemachineVirtualCameraBase CurrentCamera;

    [SerializeField] private Transform followTarget;

    public Transform ForwardRotation;
    public Vector3 camViewDirection; //Facing towards player

    [SerializeField] private float MouseSensitivity;
    private float defaultMouseSensitivity;

    private Vector2 mouseVector;
    private float xInput;
    private float yInput;

    private Vector2 screenCenter;
    private Vector3 aimPoint;

    private bool aimMode;
    private bool canAim;

    [SerializeField] private Transform targetTransform;

    [Header("Reference to Player Input")]
    [SerializeField] private InputManager playerInputs;

    public event EventHandler<bool> OnAimMode;

    private void Awake()
    {
        if (thirdPersonCam == null)
        {
            Debug.LogWarning("No ThirdPersonCam detected");
        }
        if (aimCamera == null)
        {
            Debug.LogWarning("No AimRightCam detected");
        }

        if (followTarget == null)
        {
            Debug.LogWarning("No Follow Target");
        }

        if (playerInputs == null)
        {
            Debug.LogWarning("No Player Input referenced");
        }
    }
    void Start()
    {
        mainCam = Camera.main;
        aimMode = false;
        canAim = true;

        aimCamera.gameObject.SetActive(false);
        CurrentCamera = thirdPersonCam;

        playerInputs.input.Player.Aim.performed += OnAimPerformed;
        playerInputs.input.Player.Aim.canceled += OnAimStopped;

        playerInputs.input.Player.MouseAIm.performed += OnMouseMoved;

        PlayerState.AllowAim += CanAim; // Dependency on PlayerState


        defaultMouseSensitivity = MouseSensitivity;
        
    }

    //Subscribed to the PlayerState aim event
    private void CanAim(object sender, bool e)
    {
        canAim = e;
    }

    private void OnMouseMoved(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        mouseVector = obj.ReadValue<Vector2>();
        yInput += mouseVector.x * (MouseSensitivity/10); 
        xInput += mouseVector.y * (MouseSensitivity/10);        
    }

    private void OnAimStopped(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //thirdPersonCam.gameObject.SetActive(true);
        //aimCamera.gameObject.SetActive(false);

        aimMode = false;
        SwitchCams(aimMode);

        CurrentCamera = thirdPersonCam;
    }

    private void OnAimPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (canAim)
        {
            //thirdPersonCam.gameObject.SetActive(false);
            //aimCamera.gameObject.SetActive(true);

            aimMode = true;
            SwitchCams(aimMode);
            CurrentCamera = aimCamera;
        }        
    }

    void Update()
    {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        camViewDirection = (gameObject.transform.position - new Vector3(CurrentCamera.transform.position.x,
                         gameObject.transform.position.y, CurrentCamera.transform.position.z)).normalized;

        MouseSensitivity = defaultMouseSensitivity;

        if (aimMode)
        {
            if (canAim)
            {
                SetAimPoint();
            }
            else
            {
                SwitchCams(false);
            }
        }

        ForwardRotation.forward = camViewDirection;
    }

    //Since we are using fixedUpdate movement, Camera must be the same
    private void FixedUpdate()
    {
        rotateVirtualCam();


        //Debug.DrawRay(CurrentCamera.transform.position, camViewDirection * 10, Color.magenta);
    }
    private void SetAimPoint()
    {
        Ray ray = mainCam.ScreenPointToRay(screenCenter);

        Debug.DrawRay(CurrentCamera.transform.position, ray.direction * 50f, Color.magenta);

        aimPoint = mainCam.ScreenToWorldPoint(screenCenter) + ray.direction * 50f;

        camViewDirection = (new Vector3(aimPoint.x, 0, aimPoint.z) - new Vector3(gameObject.transform.position.x,
                     0, gameObject.transform.position.z)).normalized;

        if (targetTransform != null)
        {
            targetTransform.position = aimPoint;
        }

        MouseSensitivity /= 2;
    }
    
    private void SwitchCams(bool aim)
    {
        thirdPersonCam.gameObject.SetActive(!aim);
        aimCamera.gameObject.SetActive(aim);

        CurrentCamera = (thirdPersonCam.enabled) ? thirdPersonCam : aimCamera;
    }

    private void rotateVirtualCam()
    {
        xInput = Mathf.Clamp(xInput, -30, 30);
        Quaternion rotation = Quaternion.Euler(-xInput, yInput, 0f);
        followTarget.rotation = rotation;
    }

    public Vector3 GetAimPoint()
    {
        return aimPoint;
    }
}
