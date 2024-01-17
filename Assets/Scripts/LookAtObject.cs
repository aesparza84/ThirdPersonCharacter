using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint headRig;
    [SerializeField] private Transform headAimTarget;

    private Vector3 localDefaultPosition;
    [SerializeField] private float aimSpeed;

    private bool doneLooking;

    void Start()
    {       
        headRig = GetComponent<MultiAimConstraint>();
        localDefaultPosition = headAimTarget.localPosition;   
        doneLooking = false;
    }

    void Update()
    {
        if (doneLooking && headRig.weight != 0)
        {
            setWeight(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Important"))
        {
            changeAimTargert(other.gameObject.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Important"))
        changeAimTargert(other.gameObject.transform.position);
    }

    private void changeAimTargert(Vector3 newPosition)
    {
        doneLooking = false;

        headAimTarget.position = Vector3.Lerp(headAimTarget.position,
                                 newPosition, Time.deltaTime * aimSpeed);
        
        setWeight(1);        
    }

    private void setWeight(int newWeight)
    {
        headRig.weight = Mathf.Lerp(headRig.weight, newWeight, Time.deltaTime * aimSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Important"))
        {            
            doneLooking = true;
        }
    }
}
