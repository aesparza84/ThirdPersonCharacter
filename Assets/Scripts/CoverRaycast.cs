using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverRaycast : MonoBehaviour
{
    [Header("Cover Check Position")]
    [SerializeField] private Transform CoverStart;
    private Vector3 rayStart;
    private Vector3 rayDirection;

    [Header("Forward Reference")]
    [SerializeField] private Transform playerBody;

    [SerializeField] private float rayDistance;

    [SerializeField] private LayerMask coverMask;

    public Vector3 CoverPoint;

    public bool NormalOnX, NormalOnZ;

    private void Awake()
    {
        if (CoverStart == null)
        {
            Debug.LogWarning("CoverCast Pos not set");
        }
        else
        {
            rayStart = CoverStart.position;
        }

        if (playerBody == null)
        {
            Debug.LogWarning("Cover cam ref not set");
        }
        else
        {
            rayDirection = playerBody.forward;
        }
    }

    void Start()
    {
        NormalOnX = false; 
        NormalOnZ = false;
        //if (CoverStart)
        //{
        //    rayStart = CoverStart.position;
        //}

        //if (cam)
        //{
        //    rayDirection = cam.camViewDirection;
        //}
    }

    private void Update()
    {
        setRaypos();
        Debug.DrawRay(rayStart, rayDirection * rayDistance, Color.cyan);
    }

    private void setRaypos()
    {
        rayStart = CoverStart.position;
        rayDirection = playerBody.forward;

    }
    public bool LookForCover()
    {
        CoverPoint = Vector3.zero;
        RaycastHit coverCheck;
        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, rayDistance, coverMask))
        {
            coverCheck = hit;
            CoverPoint = coverCheck.point;

            //Checking Perpendicular/ normal
            if (Mathf.Abs(coverCheck.normal.x) == 1)
            {
                NormalOnX = true;
            }
            if (Mathf.Abs(coverCheck.normal.z) == 1)
            {
                NormalOnZ = true;
            }

            Debug.Log("Hit Normal: " +coverCheck.normal);
            return true;
        }
        return false;
    }

    public Vector3 GetPoint()
    {
        return CoverPoint;
    }
}
