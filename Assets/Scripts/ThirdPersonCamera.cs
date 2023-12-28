using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References to Player Body")]
    [SerializeField] private GameObject Player;
    private Rigidbody myBody;

    private InputManager inputHandler;
    private float HorizontalInput;
    private float VerticalInput;

    public Transform forwaredRotation;
    public Vector3 camViewDirection;

    private void Start()
    {
        if (inputHandler == null)
        {
            inputHandler = GetComponentInParent<InputManager>();
        }
        inputHandler.OnMovementPerf += MovementRecieved;

        if (myBody == null)
        {
            //myBody = Player.GetComponent<Rigidbody>();
            myBody = GetComponentInParent<Rigidbody>();
        }
    }

    private void MovementRecieved(object sender, Vector2 e)
    {
        //Get float values
        HorizontalInput = e.x;
        VerticalInput = e.y;
    }

    private void Update()
    {
        camViewDirection = (Player.transform.position - new Vector3(gameObject.transform.position.x, 
                         Player.transform.position.y, gameObject.transform.position.z) ).normalized;
   
        forwaredRotation.forward = camViewDirection;

        //myBody.transform.forward = camViewDirection;

        Debug.DrawRay(gameObject.transform.position, camViewDirection* 10, Color.green);
    }
}
