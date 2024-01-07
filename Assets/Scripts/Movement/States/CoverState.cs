using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverState : MovingState
{

    private Vector3 coverNormal, crossVector;
    private Vector3 playerPos;
    private Transform playerTransform;
    private Vector3 colliderPosLHS, colliderPosRHS, startingLHS, startingRHS;

    private float colliderWidth, colliderHeight, colliderZ;

    public CoverState(MoveStateManager context)
    {
        context.StoppedCover += OnLeaveCover;

        colliderWidth = context.playerCollider.bounds.extents.x;
        colliderHeight = context.playerCollider.bounds.extents.y;
        colliderZ = context.playerCollider.bounds.extents.z;

        playerTransform = context.gameObject.transform;

        UsesFixedUpdt = true;
    }

    private void OnLeaveCover(object sender, MoveStateManager e)
    {
        if (active)
        {
            Debug.Log("Left Cover");
            e.inCover = false;
            e.switctStates(e.idleState);
        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        ///Shoot out RayCasts at Players-Collider's width (extents?)
        ///to detect if we passed a wall's edge
        ///

        context.MyAnimator.SetInteger("CoverHorizontal", (int)context.HorizontalInput);
        SetForwards(context);

        //RaycastHit hit;

        ///Player positon + (collider extnents); player position is alwys being updated

        playerPos = playerTransform.position;
        //playerPos = context.playerCollider.gameObject.transform.position;
        //Debug.DrawRay(playerPos, Vector3.up * 5, Color.magenta);

        colliderPosLHS = new Vector3(playerPos.x + -context.playerCollider.bounds.extents.x,
                                         playerPos.y + context.playerCollider.bounds.extents.y,
                                        playerPos.z + -context.playerCollider.bounds.extents.z);

        colliderPosRHS = new Vector3(playerPos.x + context.playerCollider.bounds.extents.x,
                                          playerPos.y + context.playerCollider.bounds.extents.y,
                                        playerPos.z + context.playerCollider.bounds.extents.z);

        colliderPosLHS = new Vector3(-colliderWidth, colliderHeight, 0);
        colliderPosRHS = new Vector3(colliderWidth, colliderHeight, 0);

        startingLHS = context.transform.TransformPoint(colliderPosLHS);
        startingRHS = context.transform.TransformPoint(colliderPosRHS);

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


    }

    private void SetForwards(MoveStateManager context)
    {
        context.PlayerBody.transform.forward = -context.coverRayCast.GetPoint().normal;
        context.physicalBodyTransform.forward = context.coverRayCast.GetPoint().normal;
        context.playerCollider.gameObject.transform.forward = -context.coverRayCast.GetPoint().normal;

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

        MoveToCover(context);
       

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
        context.PlayerBody.MovePosition(context.coverRayCast.CoverPoint);
    }

    public override void ExitState(MoveStateManager context)
    {
        context.MyAnimator.SetBool("IsCover", false);
        context.physicalBodyTransform.forward = context.gameObject.transform.forward;
        context.coverRayCast.NormalOnX = false;
        context.coverRayCast.NormalOnZ = false;
    }
}
