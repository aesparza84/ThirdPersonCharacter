using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCover : PlayerState
{
    private Transform playerTransform;

    private Vector3 colliderPosLHS, colliderPosRHS, startingLHS, startingRHS;
    private Vector3 crossVector;
    private Vector3 LowerLeft, LowerRight;

    private RaycastHit CurrentWallHit;

    private bool hitLeft, hitRight;
    private float colliderOffest;

    private float prevHorizontal;

    bool reachedCover;

    public PlayerCover(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory) : base(passedContext, passedFactory)
    {
        
    }

    public override void SwitchConditions()
    {
        if (_context.CoverPressed)
        {
            if (_context.CrouchedCover)
            {
                SwitchToState(_factory.Crouch());
            }
            else
            {
                SwitchToState(_factory.Idle());
            }
        }
        else if (aiming)
        {
            _context.Crouched = false;
            _context.CrouchedCover = false;
            SwitchToState(_factory.Idle());
        }
    }

    public override void ChooseSubState()
    {

    }

    public override void EnterState()
    {
        colliderOffest = 0.2f;

        RaiseAimEvent(false);

        playerTransform = _context.gameObject.transform;
        //-------//
        _context.CoverPressed = false;

        _context.ToggleColliders(!_context.CrouchedCover, _context.CrouchedCover);
        _context.MyAnimator.SetBool("IsCrouching", _context.CrouchedCover);

        MoveToCover();
        colliderPosLHS = new Vector3(-_context.ColliderWidth + colliderOffest, _context.ColliderHeight * 0.5f, 0);
        colliderPosRHS = new Vector3(_context.ColliderWidth - colliderOffest, _context.ColliderHeight * 0.5f, 0);

        speed = 0;

        //Check the cover ray cast normal hit
        //Restrict RigidBody to perpdenicular axis       
        

        ToggleAnimationBool(true);
    }

    public override void ExitState()
    {
        _context.PhyscialBodyTransfom.forward = playerTransform.forward;
        _context.CoverPressed = false;
        _context.CrouchedCover = false;
        _context.MyAnimator.SetBool("IsCrouching", _context.CrouchedCover);
        ToggleAnimationBool(false);
    }

    public override void FixedUpdate()
    {
        _context.MoveVector = _context.PlayerTransform.right * _context.HorizontalIput +
                             _context.PlayerTransform.up * _context.VerticalIput;

        //This works by setting the rigidbody forward to face wall in Updateaction
        //Thats why we move along the rigidbodies transfom.Right

        crossVector = _context.PlayerBody.transform.right;
        _context.MoveVector = Vector3.Project(_context.MoveVector.normalized, crossVector);

        #region Using Projection and Cross to limit movemont on axis  (Doesn't work)
        //crossVector = Vector3.Cross(context.moveVector.normalized, coverNormal.normalized);
        //context.moveVector = Vector3.Cross(context.moveVector.normalized, coverNormal.normalized);
        //context.moveVector = Vector3.Project(cancelledAxis, context.moveVector);
        #endregion
        //_context.PlayerBody.velocity = _context.MoveVector.normalized * _context.Currentspeed;
    }

    public override void Update()
    {
        SwitchConditions();

        if (_context.IsMoving)
        {
            _context.Currentspeed = _context.CoverSpeed;
        }
        else
        {
            _context.Currentspeed = 0;
        }

        if (_context.CrouchPressed)
        {
            _context.CrouchPressed = false;
            ToggleCrouchCover();
        }

        ///Shoot out RayCasts at Players-Collider's width (extents?)
        ///to detect if we passed a wall's edge
        ///

        SpeedAndLeaningDirection();

        SetForwards();
        EdgeStopAndTransition(2);

        //if (coverCrouch != context.crouched)
        //{
        //    coverCrouch = context.crouched;
        //}
        ///Loacl positon + (collider extnents); local position is relative to transform i want to use
        ///


        startingLHS = _context.transform.TransformPoint(colliderPosLHS);
        startingRHS = _context.transform.TransformPoint(colliderPosRHS);

        hitLeft = Physics.Raycast(startingLHS, _context.gameObject.transform.forward, 1);
        hitRight = Physics.Raycast(startingRHS, _context.gameObject.transform.forward, 1);
        //StopOnEdge();

        #region Debugging
        //Debug.Log("HitLeft: " + hitLeft);
        //Debug.Log("HitRight: " + hitRight);

        Debug.DrawRay(startingLHS, _context.gameObject.transform.forward, Color.green);
        Debug.DrawRay(startingRHS, _context.gameObject.transform.forward, Color.magenta);

        //Drawing the normal
        //Debug.DrawRay(_context.StandingCollider.transform.position,
        //    (_context.StandingCollider.transform.position - _context.CoverRayCast.CoverPoint).normalized * 3,
        //    Color.green);

        ////Drawing the Cross vector, parallel to the wall we are on covered
        //Debug.DrawRay(_context.StandingCollider.transform.position,
        //    (_context.StandingCollider.transform.position - _context.CoverRayCast.CoverPoint).normalized * 3,
        //    Color.blue);
        #endregion
    }

    protected override void ToggleAnimationBool(bool toggle)
    {
        _context.MyAnimator.SetBool("IsCover", toggle);
    }

    private void MoveToCover()
    {
        _context.StartCoroutine(getToCover(_context.PlayerBody,
                                          _context.PlayerBody.position,
                                          _context.CoverRayCast.GetCoverPoint(),
                                          1.3f));
    }

    private IEnumerator getToCover(Rigidbody playerBody, Vector3 playerPos, RaycastHit finalPos, float lerpSpeed)
    {
        float t = 0;
        Vector3 currentPos = playerPos;
        Vector3 finalVector = new Vector3(finalPos.point.x, currentPos.y, finalPos.point.z);

        playerBody.isKinematic = true;
        while (t < 1)
        {
            playerPos = Vector3.Lerp(currentPos, finalVector, t);
            t += Time.deltaTime * lerpSpeed;
            playerBody.MovePosition(playerPos);

            yield return null;
            reachedCover = true;
        }
        playerBody.isKinematic = false;

        RaiseAimEvent(reachedCover);

        playerTransform.forward = -finalPos.normal;
        _context.PhyscialBodyTransfom.forward = -playerTransform.forward;
    }
    private void SetForwards()
    {
        if (Physics.Raycast(_context.gameObject.transform.position, _context.gameObject.transform.forward,
                            out CurrentWallHit, 3.0f, _context.CoverMask))
        {
            _context.PlayerBody.transform.forward = -CurrentWallHit.normal;

        }
    }
    private void EdgeStopAndTransition(float transitionDistance)
    {
        LowerLeft = _context.transform.TransformPoint(new Vector3(colliderPosLHS.x-0.05f, 1, colliderPosLHS.z));
        LowerRight = _context.transform.TransformPoint(new Vector3(colliderPosRHS.x+0.05f, 1, colliderPosRHS.z));


        Debug.DrawRay(LowerLeft, _context.gameObject.transform.forward * transitionDistance, Color.red);
        Debug.DrawRay(LowerRight, _context.gameObject.transform.forward * transitionDistance, Color.red);

        if (!hitLeft)
        {
            if (Physics.Raycast(LowerLeft, _context.gameObject.transform.forward, out RaycastHit lHit, transitionDistance))
            {
                _context.StartCoroutine(getToCover(_context.PlayerBody, _context.transform.position, lHit, 3f));
            }
            else
            {
                _context.HorizontalIput = Mathf.Clamp(_context.HorizontalIput, 0, 1);
            }
        }
        else if (!hitRight)
        {
            if (Physics.Raycast(LowerRight, _context.gameObject.transform.forward, out RaycastHit rHit, transitionDistance))
            {
                _context.StartCoroutine(getToCover(_context.PlayerBody, _context.transform.position, rHit, 3f));
            }
            else
            {
                _context.HorizontalIput = Mathf.Clamp(_context.HorizontalIput, -1, 0);
            }
        }
    }
    private void ToggleCrouchCover()
    {
        _context.CrouchedCover = !_context.CrouchedCover;
        _context.ToggleColliders(!_context.CrouchedCover, _context.CrouchedCover);
        _context.MyAnimator.SetBool("IsCrouching", _context.CrouchedCover);
    }
    private void SpeedAndLeaningDirection()
    {
        //Runs if we are giving input
        if (_context.HorizontalIput != 0 && speed < _context.CoverSpeed)
        {
            speed += Time.deltaTime * 5f;
            speed = Mathf.Clamp(speed, 0, _context.CoverSpeed);
        }

        //Below is just for Animation
        if (_context.HorizontalIput > 0)
        {
            prevHorizontal += Time.deltaTime * 5f;
            prevHorizontal = Mathf.Clamp(prevHorizontal, 0, 1);
        }
        else if (_context.HorizontalIput < 0)
        {
            prevHorizontal -= Time.deltaTime * 5f;
            prevHorizontal = Mathf.Clamp(prevHorizontal, -1, 0);
        }

        //Runs if we are not fully stopped
        if (_context.HorizontalIput == 0 && speed > 0)
        {
            speed -= Time.deltaTime * 5f;
            speed = Mathf.Clamp(speed, 0, _context.CoverSpeed);

            //Below is just for Animation
            if (prevHorizontal > 0)
            {
                prevHorizontal -= Time.deltaTime * 5f;
                prevHorizontal = Mathf.Clamp(prevHorizontal, 0, 1);
            }
            else if (prevHorizontal < 0)
            {
                prevHorizontal += Time.deltaTime * 5f;
                prevHorizontal = Mathf.Clamp(prevHorizontal, -1, 0);
            }
        }

        _context.MyAnimator.SetFloat("Horizontal", prevHorizontal);
        _context.Currentspeed = speed;
    }
}
