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

    [Header("Climb and Vault Heights")]
    [Tooltip("Climb: Player Height is multipled by this to determine how tall an object can be to alllow climb.\n" +
             "Vault: How long we will allow an obstacle to be considered 'vaultable.'")]
    [SerializeField] private float climbMax;
    [SerializeField] private float vaultMax;


    [Header("Forward Reference")]
    [SerializeField] private Transform playerBody;

    [SerializeField] private float rayDistance;

    [SerializeField] private LayerMask coverMask;

    public Vector3 CoverPoint;
    private Vector3 climbPoint;
    private Vector3 vaultPoint;

    public bool CanClimb;
    public bool CanVault;

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
        playerHeight = playerCollider.bounds.extents.y * 2f;
        vaultPoint = Vector3.zero;
        climbPoint = Vector3.zero;
    }

    private void Update()
    {
        setRaypos();
        Debug.DrawRay(rayStart, rayDirection * rayDistance, Color.cyan);

        if (Physics.Raycast(rayStart, gameObject.transform.forward.normalized, out RaycastHit hit, 1.7f, coverMask))
        {
            CanClimb = CheckClimbable(climbMax, hit);
            CanVault = CheckVaultable(vaultMax, hit);

            if (debugTransform != null)
            {
                drawClimbRays(climbMax, hit);
                drawVaultRays(vaultMax, hit);
            }            
        }
        else
        {
            CanClimb = false;
            CanVault = false;
        }

        #region Debugging
        //Debug.Log("Can Climb: "+ CheckClimbable(climbMax));
        //Debug.Log("Can Vault: "+ CheckVaultable(vaultMax));
        #endregion
    }

    private void drawClimbRays(float heightCheck, RaycastHit hit)
    {
        Debug.DrawRay(rayStart, gameObject.transform.forward.normalized * 1.7f, Color.magenta);

        Vector3 insidePoint = hit.point + gameObject.transform.forward.normalized * playerRadius;
        Vector3 topPoint = new Vector3(insidePoint.x, insidePoint.y + playerHeight * heightCheck, insidePoint.z);

        Debug.DrawRay(hit.point, gameObject.transform.forward.normalized * playerRadius, Color.green);
        Debug.DrawRay(topPoint, Vector3.down.normalized * playerHeight, Color.cyan);

        if (debugTransform != null)
        {
            //debugTransform.position = climbPoint;
        }
    }

    private void drawVaultRays(float maxSlideLength, RaycastHit hit)
    {
        //Shoot ray from player Collider:MIddle and Top-Height
        //Shoot ray Vector3.down to see if you hit ground
        Vector3 middle = new Vector3(rayStart.x, rayStart.y + playerCollider.bounds.extents.y, rayStart.z);

        Vector3 fromHitPoint = new Vector3(hit.point.x, middle.y, hit.point.z);

        Debug.DrawRay(middle, gameObject.transform.forward.normalized * maxSlideLength, Color.green);

        //Checks if our 'middle' isn't collding
        if (!Physics.Raycast(middle, gameObject.transform.forward.normalized, 1.7f))
        {
            //Shoots a ray from the hit point to (direction * maxSlide)
            if (!Physics.Raycast(fromHitPoint, gameObject.transform.forward, maxSlideLength))
            {
                Vector3 downPoint = fromHitPoint + (gameObject.transform.forward.normalized * maxSlideLength);
                Debug.DrawRay(fromHitPoint, gameObject.transform.forward.normalized * maxSlideLength);

                //Shoots ray Downward to see if obstacle goes past allowed length
                if (!Physics.Raycast(downPoint, Vector3.down, 1))
                {
                    Debug.DrawRay(downPoint, Vector3.down * 1, Color.green);
                    debugTransform.position = downPoint;
                }
            }
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
    /// Climb: we would set this to be (roughly) 1.5 -> 1.7 times the player height
    /// to check if we need to climb on top of the object of reasonable height.
    /// 
    /// We can prioritize vaulting since both will be true when vault is true.
    /// 
    /// </summary>
    /// <param name="heightCheck"></param>
    /// <returns></returns>
    private bool CheckClimbable(float heightCheck, RaycastHit hit)
    {
        climbPoint = Vector3.zero;
        Vector3 insidePoint = hit.point + gameObject.transform.forward.normalized * playerRadius;
        Vector3 topPoint = new Vector3(insidePoint.x, insidePoint.y + playerHeight * heightCheck, insidePoint.z);

        if (Physics.Raycast(topPoint, Vector3.down, out RaycastHit newPoint, playerHeight * heightCheck, coverMask))
        {
            climbPoint = newPoint.point;
            return true;
        }

        return false;
    }

    private bool CheckVaultable(float maxSlideLength, RaycastHit hit)
    {
        vaultPoint = Vector3.zero;

        //Shoot ray from player Collider: Middle 
        //Shoot ray Vector3.down to see if it clears object of length
        Vector3 middle = new Vector3(rayStart.x, rayStart.y + playerCollider.bounds.extents.y, rayStart.z);

        Vector3 fromHitPoint = new Vector3(hit.point.x, middle.y, hit.point.z);

        //Checks if our 'middle' isn't collding
        if (!Physics.Raycast(middle, gameObject.transform.forward.normalized, 1.7f))
        {
            //Shoots a ray from the hit point to (direction * maxSlide)
            if (!Physics.Raycast(fromHitPoint, gameObject.transform.forward, maxSlideLength))
            {
                Vector3 downPoint = fromHitPoint + (gameObject.transform.forward.normalized * maxSlideLength);
                //Debug.DrawRay(fromHitPoint, gameObject.transform.forward.normalized * maxSlideLength);

                //Shoots ray Downward to see if obstacle goes past allowed length
                if (!Physics.Raycast(downPoint, Vector3.down, 1))
                {
                    //Debug.DrawRay(downPoint, Vector3.down * 1, Color.green);
                    //debugTransform.position = downPoint;

                    vaultPoint = middle + gameObject.transform.forward.normalized * maxSlideLength;

                    return true;
                }
            }
        }

        return false;
    }
    public RaycastHit GetCoverPoint()
    {
        return coverCheck;
    }

    public Vector3 GetClimbPoint()
    {
        return climbPoint;
    }

    public Vector3 GetVaultPoint() 
    {
        return vaultPoint; 
    }

}
