using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverState : MovingState
{

    private Vector3 coverNormal, crossVector;
    private Vector3 playerPos;
    private Transform playerTransform;

    private Vector3 colliderPosLHS, colliderPosRHS, startingLHS, startingRHS;

    private bool hitLeft, hitRight;

    private float colliderWidth, colliderHeight, colliderZ;
    private float colliderOffest;

    private bool coverCrouch;

    public CoverState(MoveStateManager context)
    {
        context.StoppedCover += OnLeaveCover;
        context.StartedCrouch += OnCrouch;
        coverCrouch= false;

        colliderWidth = context.StandingCollider.bounds.extents.x;
        colliderHeight = context.StandingCollider.bounds.extents.y;
        colliderZ = context.StandingCollider.bounds.extents.z;

        colliderOffest = 0.2f;

        playerTransform = context.gameObject.transform;

        UsesFixedUpdt = true;
    }

    private void OnCrouch(object sender, MoveStateManager e)
    {

    }

    private void OnLeaveCover(object sender, MoveStateManager e)
    {
        if (active)
        {
            Debug.Log("Left Cover");
            e.inCover = false;

            if (coverCrouch)
            {
                e.switctStates(e.crouchState);
            }
            else
            {
                e.switctStates(e.idleState);
            }
        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        ///Shoot out RayCasts at Players-Collider's width (extents?)
        ///to detect if we passed a wall's edge
        ///

        context.MyAnimator.SetInteger("CoverHorizontal", (int)context.HorizontalInput);
        SetForwards(context);

        ///Loacl positon + (collider extnents); local position is relative to transform i want to use
        ///


        startingLHS = context.transform.TransformPoint(colliderPosLHS);
        startingRHS = context.transform.TransformPoint(colliderPosRHS);

        hitLeft = Physics.Raycast(startingLHS, context.gameObject.transform.forward, 1);
        hitRight = Physics.Raycast(startingRHS, context.gameObject.transform.forward, 1);
        StopOnEdge(context);

        #region Debugging
        /*
        Debug.Log("HitLeft: " + hitLeft);
        Debug.Log("HitRight: " + hitRight);

        Debug.DrawRay(startingLHS, context.gameObject.transform.forward, Color.green);
        Debug.DrawRay(startingRHS, context.gameObject.transform.forward, Color.magenta);

        //Drawing the normal
        Debug.DrawRay(context.playerCollider.transform.position,
            (context.playerCollider.transform.position - context.coverRayCast.CoverPoint).normalized * 3,
            Color.green);

        //Drawing the Cross vector, parallel to the wall we are on covered
        Debug.DrawRay(context.playerCollider.transform.position,
            (context.playerCollider.transform.position - context.coverRayCast.CoverPoint).normalized * 3,
            Color.blue);
        */
        #endregion
    }

    private void StopOnEdge(MoveStateManager context)
    {
        if (!hitLeft)
        {
            context.HorizontalInput = Mathf.Clamp(context.HorizontalInput, 0, 1);
        }
        if (!hitRight)
        {
            context.HorizontalInput = Mathf.Clamp(context.HorizontalInput, -1, 0);
        }
    }

    private void SetForwards(MoveStateManager context)
    {
        context.PlayerBody.transform.forward = -context.coverRayCast.GetPoint().normal;
        context.physicalBodyTransform.forward = context.coverRayCast.GetPoint().normal;
        context.StandingCollider.gameObject.transform.forward = -context.coverRayCast.GetPoint().normal;

        playerTransform.forward = -context.coverRayCast.GetPoint().normal;
    }

    public override void DoFixedUpate(MoveStateManager context)
    {
        context.moveVector = context.direction.right * context.HorizontalInput + 
                             context.direction.up * context.VerticalInput;

        //This works by setting the rigidbody forward to face wall in Updateaction
        //Thats why we move along the rigidbodies transfom.Right

        playerTransform = context.gameObject.transform;

        crossVector = context.PlayerBody.transform.right;
        context.moveVector = Vector3.Project(context.moveVector.normalized, crossVector);

        #region Using Projection and Cross to limit movemont on axis  (Doesn't work)
        //crossVector = Vector3.Cross(context.moveVector.normalized, coverNormal.normalized);
        //context.moveVector = Vector3.Cross(context.moveVector.normalized, coverNormal.normalized);
        //context.moveVector = Vector3.Project(cancelledAxis, context.moveVector);
        #endregion
        context.PlayerBody.velocity = context.moveVector.normalized * context.Currentspeed;
    }

    public override void EnterState(MoveStateManager context)
    {
        //We enter the 'covered' state
        active = true;
        context.MyAnimator.SetBool("IsCover", true);

        coverCrouch = context.crouched;
        if (coverCrouch)
        {
            context.MyAnimator.SetBool("IsCrouching", true);
        }

        MoveToCover(context);

        colliderPosLHS = new Vector3(-colliderWidth + colliderOffest, colliderHeight * 0.5f , 0);
        colliderPosRHS = new Vector3(colliderWidth - colliderOffest, colliderHeight * 0.5f, 0);

        context.inCover = true;
        context.Currentspeed = context.CoverSpeed;

        //Check the cover ray cast normal hit
        //Restrict RigidBody to perpdenicular axis

        coverNormal = context.coverRayCast.GetPoint().normal;
        Debug.Log("Cncelled normal: "+coverNormal);

        #region Old vector projecting
        //if (context.coverRayCast.NormalOnX)
        //{
        //    Debug.Log("Stopping X movement");
        //    cancelledAxis = Vector3.right;
        //}
        //else if (context.coverRayCast.NormalOnZ)
        //{
        //    Debug.Log("Stopping Z movement");
        //    cancelledAxis = Vector3.up;
        //}
        #endregion

    }
    private void MoveToCover(MoveStateManager context)
    {
        context.StartCoroutine(getToCover(context.PlayerBody, 
                                          context.PlayerBody.position,
                                          context.coverRayCast.CoverPoint,
                                          1.3f));
        
        //context.PlayerBody.MovePosition(context.coverRayCast.CoverPoint);
    }

    private IEnumerator getToCover(Rigidbody playerBody, Vector3 playerPos, Vector3 finalPos, float lerpSpeed)
    {        
        float t = 0;
        Vector3 currentPos = playerPos;

        while(t < 1)
        {
            playerPos = Vector3.Lerp(currentPos, finalPos, t);
            t += Time.deltaTime * lerpSpeed;
            playerBody.MovePosition(playerPos);
            
            yield return null;            
        }
    }

    public override void ExitState(MoveStateManager context)
    {
        context.MyAnimator.SetBool("IsCover", false);
        context.physicalBodyTransform.forward = context.gameObject.transform.forward;
    }
}
