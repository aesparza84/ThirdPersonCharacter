using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverRaycast : MonoBehaviour
{
    [Header("Cover Check Position")]
    [SerializeField] private Transform CoverStart;
    private Vector3 rayStart;
    private Vector3 rayDirection;
    private RaycastHit coverCheck;


    [Header("Forward Reference")]
    [SerializeField] private Transform playerBody;

    [SerializeField] private float rayDistance;

    [SerializeField] private LayerMask coverMask;

    public Vector3 CoverPoint;
    private Vector3 vaultPoint;

    [SerializeField] private Collider playerCollider;
    [SerializeField] private float playerRadius;
    [SerializeField] private float playerHeight;
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

        if (playerCollider == null)
        {
            Debug.LogWarning("Collider ref not set");
        }
    }

    void Start()
    {
        playerRadius = playerCollider.bounds.extents.x;
        playerHeight = playerCollider.bounds.extents.y * 1.5f;
    }

    private void Update()
    {
        setRaypos();
        Debug.DrawRay(rayStart, rayDirection * rayDistance, Color.cyan);

        drawVaultRays();
    }

    private void drawVaultRays()
    {
        Vector3 point = Vector3.zero;

        Debug.DrawRay(rayStart, gameObject.transform.forward.normalized * 1.0f, Color.red);

        if (Physics.Raycast(rayStart, gameObject.transform.forward, out RaycastHit hit, 1.0f, coverMask))
        {
            Vector3 insidePoint = hit.point + gameObject.transform.forward.normalized * playerRadius;
            Vector3 topPoint = new Vector3(insidePoint.x, insidePoint.y + playerHeight, insidePoint.z);

            Debug.DrawRay(insidePoint, gameObject.transform.forward.normalized * playerRadius, Color.green);
            Debug.DrawRay(topPoint, Vector3.down.normalized * playerHeight, Color.cyan);
        }
    }

    private void setRaypos()
    {
        rayStart = CoverStart.position;
        rayDirection = playerBody.forward;

    }
    public bool LookForCover()
    {
        CoverPoint = Vector3.zero;
        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, rayDistance, coverMask))
        {
            coverCheck = hit;
            CoverPoint = coverCheck.point;

            //Debug.Log("Hit Normal: " +coverCheck.normal);
            return true;
        }
        return false;
    }

    public bool CanVault()
    {
        vaultPoint = Vector3.zero;
        if (Physics.Raycast(rayStart, gameObject.transform.forward, out RaycastHit hit, 1.0f, coverMask))
        {
            Vector3 insidePoint = hit.point + gameObject.transform.forward.normalized * playerRadius;
            Vector3 topPoint = new Vector3(insidePoint.x, insidePoint.y + playerHeight, insidePoint.z);

            if (Physics.Raycast(topPoint, Vector3.down, out RaycastHit newPoint, playerHeight))
            {
                vaultPoint = newPoint.point;
                return true;
            }
        }

        return false;
    }
    public RaycastHit GetCoverPoint()
    {
        return coverCheck;
    }

    public Vector3 GetVaultPoint()
    {
        return vaultPoint;
    }

}
