using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("The Players Cameras")]
    [SerializeField] private CinemachineVirtualCameraBase thirdPersonCam;
    [SerializeField] private CinemachineVirtualCameraBase aimCamera;
    //[SerializeField] private CinemachineVirtualCameraBase AimLeftCam;

    private CinemachineVirtualCameraBase CurrentCamera;

    [SerializeField] private Transform followTarget;

    public Transform ForwardRotation;
    public Vector3 camViewDirection; //Facing towards player

    [SerializeField] private float MouseSensitivity;
    private float defaultMouseSensitivity;

    private Vector2 mouseVector;
    private float xInput;
    private float yInput;

    bool aimMode;

    [Header("Reference to Player Input")]
    [SerializeField] private InputManager playerInputs;


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
        aimCamera.gameObject.SetActive(false);
        CurrentCamera = thirdPersonCam;

        playerInputs.input.Player.Aim.performed += OnAimPerformed;
        playerInputs.input.Player.Aim.canceled += OnAimStopped;

        playerInputs.input.Player.MouseAIm.performed += OnMouseMoved;

        defaultMouseSensitivity = MouseSensitivity;
        aimMode = false;
    }

    private void OnMouseMoved(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        mouseVector = obj.ReadValue<Vector2>();
        yInput += mouseVector.x * (MouseSensitivity/10); 
        xInput += mouseVector.y * (MouseSensitivity/10);        
    }

    private void OnAimStopped(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        thirdPersonCam.gameObject.SetActive(true);
        aimCamera.gameObject.SetActive(false);

        aimMode = false;

        CurrentCamera = thirdPersonCam;
    }

    private void OnAimPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        thirdPersonCam.gameObject.SetActive(false);
        aimCamera.gameObject.SetActive(true);

        aimMode = true;

        CurrentCamera = aimCamera;
    }

    void Update()
    {
        camViewDirection = (gameObject.transform.position - new Vector3(CurrentCamera.transform.position.x,
                         gameObject.transform.position.y, CurrentCamera.transform.position.z)).normalized;

        ForwardRotation.forward = camViewDirection;

        MouseSensitivity = defaultMouseSensitivity;
        if (aimMode)
        {
            MouseSensitivity /= 2;
        }
        rotateVirtualCam();
       


        Debug.DrawRay(CurrentCamera.transform.position, camViewDirection * 10, Color.magenta);
    }

    private void rotateVirtualCam()
    {
        xInput = Mathf.Clamp(xInput, -30, 70);
        Quaternion rotation = Quaternion.Euler(-xInput, yInput, 0f);
        followTarget.rotation = rotation;
    }
}