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

    public Transform ForwardRotation;
    public Vector3 camViewDirection; //Facing towards player

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
    }

    private void OnAimStopped(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        thirdPersonCam.gameObject.SetActive(true);
        aimCamera.gameObject.SetActive(false);

        CurrentCamera = thirdPersonCam;
    }

    private void OnAimPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        thirdPersonCam.gameObject.SetActive(false);
        aimCamera.gameObject.SetActive(true);

        CurrentCamera = aimCamera;
    }

    void Update()
    {
        camViewDirection = (gameObject.transform.position - new Vector3(CurrentCamera.transform.position.x,
                         gameObject.transform.position.y, CurrentCamera.transform.position.z)).normalized;

        ForwardRotation.forward = camViewDirection;

        Debug.DrawRay(CurrentCamera.transform.position, camViewDirection * 10, Color.magenta);
    }
}
