using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverState : MovingState
{

    private Vector3 coverNormal, crossVector;
    public CoverState(MoveStateManager context)
    {
        context.StoppedCover += OnLeaveCover;

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

        context.PlayerBody.transform.forward = -context.coverRayCast.GetPoint().normal;
        context.physicalBodyTransform.forward = context.coverRayCast.GetPoint().normal;

        //RaycastHit hit;

        Vector3 rayPosLeft = new Vector3(context.transform.position.x + -context.playerCollider.bounds.extents.x,
                                         context.transform.position.y + context.playerCollider.bounds.extents.y);

        Vector3 rayPosRight = new Vector3(context.transform.position.x + context.playerCollider.bounds.extents.x,
                                          context.transform.position.y + context.playerCollider.bounds.extents.y);
        
        Debug.DrawRay(rayPosLeft, context.gameObject.transform.forward, Color.magenta);
        Debug.DrawRay(rayPosRight, context.gameObject.transform.forward, Color.magenta);

        //Drawing the normal
        Debug.DrawRay(context.playerCollider.transform.position,
            (context.playerCollider.transform.position - context.coverRayCast.CoverPoint).normalized * 3,
            Color.green);

        //Drawing the Cross vector, parallel to the wall we are on covered
        Debug.DrawRay(context.playerCollider.transform.position,
            (context.playerCollider.transform.position - context.coverRayCast.CoverPoint).normalized * 3,
            Color.green);
    }

    public override void DoFixedUpate(MoveStateManager context)
    {
        context.moveVector = context.direction.right * context.HorizontalInput + 
                             context.direction.up * context.VerticalInput;

        //This works by setting the rigidbody forward to face wall in Updateaction
        //Thats why we move along the rigidbodies transfom.Right
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
        Debug.Log("Move to " + context.coverRayCast.CoverPoint);
    }

    public override void ExitState(MoveStateManager context)
    {
        context.MyAnimator.SetBool("IsCover", false);
        context.physicalBodyTransform.forward = context.gameObject.transform.forward;
        context.coverRayCast.NormalOnX = false;
        context.coverRayCast.NormalOnZ = false;
    }
}
