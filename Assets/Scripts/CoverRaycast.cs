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

    public Transform debugTransform;
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

        #region Debugging
        //Debug.Log("Can Vault: "+ CheckClimbOrVault(1));
        //Debug.Log("Can Climb: "+ CheckClimbOrVault(1.7f));
        #endregion
    }

    private void drawVaultRays()
    {
        Debug.DrawRay(rayStart, gameObject.transform.forward.normalized * 1.7f, Color.red);

        if (Physics.Raycast(rayStart, gameObject.transform.forward, out RaycastHit hit, 1.0f, coverMask))
        {
            Vector3 insidePoint = hit.point + gameObject.transform.forward.normalized * playerRadius;
            Vector3 topPoint = new Vector3(insidePoint.x, insidePoint.y + playerHeight, insidePoint.z);

            Debug.DrawRay(new Vector3(hit.point.x, hit.point.y + 1, hit.point.z), -gameObject.transform.forward.normalized, Color.red);

            Debug.DrawRay(hit.point, gameObject.transform.forward.normalized * playerRadius, Color.green);
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

    #region Individual vault/climb check
    /*
    public bool CanVault()
    {
        vaultPoint = Vector3.zero;
        if (Physics.Raycast(rayStart, gameObject.transform.forward, out RaycastHit hit, 1.7f, coverMask))
        {
            Vector3 insidePoint = hit.point + gameObject.transform.forward.normalized * playerRadius;
            Vector3 topPoint = new Vector3(insidePoint.x, insidePoint.y + playerHeight, insidePoint.z);

            if (Physics.Raycast(topPoint, Vector3.down, out RaycastHit newPoint, playerHeight, coverMask))
            {
                vaultPoint = newPoint.point;

                debugTransform.position = vaultPoint;

                return true;
            }
        }

        return false;
    }

    public bool CanClimb()
    {
        vaultPoint = Vector3.zero;
        if (Physics.Raycast(rayStart, gameObject.transform.forward, out RaycastHit hit, 1.7f, coverMask))
        {
            Vector3 insidePoint = hit.point + gameObject.transform.forward.normalized * playerRadius;
            Vector3 topPoint = new Vector3(insidePoint.x, insidePoint.y + playerHeight*1.4f, insidePoint.z);

            if (Physics.Raycast(topPoint, Vector3.down, out RaycastHit newPoint, playerHeight * 1.4f, coverMask))
            {
                vaultPoint = newPoint.point;

                debugTransform.position = vaultPoint;

                return true;
            }
        }

        return false;
    }
    */
    #endregion

    /// <summary>
    /// Pararmeter multiplies the players height to check whether we can 
    /// vault of climb the object in front of us.
    /// 
    /// Vault: we would set param to 1 since we dont want to vault anything
    /// taller than us.
    /// 
    /// Climb: we would set this to be (roughly) 1.5 -> 1.7 times the player height
    /// to check if we need to climb on top of the object of reasonable height.
    /// 
    /// We can prioritize vault since both will be true when vault is true.
    /// 
    /// </summary>
    /// <param name="heightCheck"></param>
    /// <returns></returns>
    public bool CheckClimbOrVault(float heightCheck)
    {
        vaultPoint = Vector3.zero;
        if (Physics.Raycast(rayStart, gameObject.transform.forward, out RaycastHit hit, 1.7f, coverMask))
        {
            Vector3 insidePoint = hit.point + gameObject.transform.forward.normalized * playerRadius;
            Vector3 topPoint = new Vector3(insidePoint.x, insidePoint.y + playerHeight * heightCheck, insidePoint.z);

            if (Physics.Raycast(topPoint, Vector3.down, out RaycastHit newPoint, playerHeight * heightCheck, coverMask))
            {
                vaultPoint = newPoint.point;

                debugTransform.position = vaultPoint;

                return true;
            }
        }

        return false;
    }
    public RaycastHit GetCoverPoint()
    {
        return coverCheck;
    }

    public Vector3 GetNewPos()
    {
        return vaultPoint;
    }

}
