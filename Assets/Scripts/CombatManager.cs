using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CombatManager : MonoBehaviour
{
    [Header("Weapon Configuring")]
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Transform camCenterPoint;
    [SerializeField] private GameObject weaponPrefab;
    private SimpleGun weapon;

    [Header("Inputs")]
    [SerializeField] private InputManager PlayerInputs;

    [Header("Camera")]
    [SerializeField] private CameraController camController;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private readonly int HoldWeaponBool = Animator.StringToHash("HoldingRifle");
    private readonly int AimWeaponBool = Animator.StringToHash("AimRifle");

    [Header("Hands Rigging Object")]
    [SerializeField] private Rig handsRig;

    private Vector3 aimDirection;

    private void Awake()
    {
        PlayerInputs = GetComponent<InputManager>();
        camController = GetComponent<CameraController>();

        if (handsRig == null)
        {
            Debug.LogWarning("HandRig not set");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        PlayerInputs.input.Player.Shoot.performed += OnShoot;

        PlayerInputs.input.Player.Aim.performed += OnAim;
        PlayerInputs.input.Player.Aim.canceled += OnAimStopped;

        GameObject gun = Instantiate(weaponPrefab, weaponTransform);
        weapon = gun.GetComponent<SimpleGun>();
    }

    private void OnAimStopped(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }

    private void OnAim(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {        

    }

    private void OnShoot(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        weapon.ShootWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        //aimDirection = camController.GetAimPoint() - camCenterPoint.position;
        //camCenterPoint.forward = aimDirection;
    }
}
