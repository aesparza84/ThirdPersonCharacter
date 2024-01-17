using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Transform camCenterPoint;
    [SerializeField] private GameObject weaponPrefab;
    private SimpleGun weapon;

    [SerializeField] private InputManager PlayerInputs;

    [SerializeField] private CameraController camController;

    private Vector3 aimDirection;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInputs = GetComponent<InputManager>();
        camController = GetComponent<CameraController>();
        PlayerInputs.input.Player.Shoot.performed += OnShoot;

        GameObject gun = Instantiate(weaponPrefab, weaponTransform);
        weapon = gun.GetComponent<SimpleGun>();
    }

    private void OnShoot(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        weapon.ShootWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        aimDirection = camController.GetAimPoint() - camCenterPoint.position;
        camCenterPoint.forward = aimDirection;
    }
}
