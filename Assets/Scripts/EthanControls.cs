using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Photon.Pun;
using Photon.Realtime;

public class EthanControls :  MonoBehaviourPunCallbacks
{
    private PhotonView myPV;
    // Start is called before the first frame update
    Animator animator;
    public GameObject healthBar;

    public float speedfactor = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float dashCooldownTimer = 3f;

    public CharacterController playerController;

    private Vector3 speedVelocity;

    private Vector3 movement;
    
    private Vector3  objectPos;

    public Transform floorCheck;
    public float floorDistance = 0.5f;
    public LayerMask floorMask;
    
    public Transform wallCheck;
    public float wallDistance = 0.3f;
    public LayerMask climbWallMask;

    public GameObject Ropegun;

    private bool isOnFloor,
        hasAirjump,
        dashing,
        dashcooldown,
        climbWall,
        walljump,
        wallgrab,
        allreadyHold,
        walljumping,
        pullingRope,
        lostWall;
    
    
    
    
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        if (myPV.IsMine)
        {
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("YouCantSeeYourSelf");
            transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("YouCantSeeYourSelf");
            transform.GetChild(2).gameObject.layer = LayerMask.NameToLayer("YouCantSeeYourSelf");
        }
        /*hasAirjump = false;
        dashing = false;
        climbWall = false;
        allreadyHold = false;
        walljumping = false;
        pullingRope = false;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (myPV.IsMine)
        {
            isOnFloor = Physics.CheckSphere(floorCheck.position, floorDistance, floorMask);

            climbWall = Physics.CheckSphere(wallCheck.position, wallDistance, climbWallMask);

            walljump = Physics.CheckSphere(wallCheck.position, wallDistance, floorMask);

            lostWall = !Physics.CheckSphere(transform.position, 1f, floorMask);


            //walljump Code
            if (walljump & !allreadyHold)
            {

                hasAirjump = true;
                wallgrab = true;
                allreadyHold = true;
                speedVelocity.y = 0;
                if (isOnFloor)
                {
                    Invoke("GrabWall", 0.05f);
                }

                Invoke("GrabWall", 1f);

            }

            if (lostWall)
            {
                wallgrab = false;
            }

            //code to keep the gravity low
            if (isOnFloor && speedVelocity.y < 0 & !climbWall)
            {
                animator.SetBool("bottom", true);
                speedVelocity.y = -3;
                allreadyHold = false;
                wallgrab = false;
            }

            //inputs
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //for animations of walking
            if (z < 0)
            {
                animator.SetFloat("walkSpeed", -1.0f);
            }
            else
            {
                animator.SetFloat("walkSpeed", 1.0f);
            }

            animator.SetFloat("Forward", z);
            animator.SetFloat("Turn", x);

            //movement left and right
            if (!dashing && !walljumping && !pullingRope)
            {
                movement = transform.right * x + transform.forward * z;
            }

            //movement disabled while beeing pulled
            if (!pullingRope)
            {
                if (!climbWall)
                {
                    playerController.Move(movement * speedfactor * Time.deltaTime);
                }
            }
            //here is where pulling is done
            else
            {
                playerController.Move(movement * 20f * Time.deltaTime);
                RopePulling(objectPos);
                if (Vector3.Distance(objectPos, floorCheck.position) < 2f)
                {
                    Ropegun.GetComponent<GrapplingHook>().CutRope();
                    StopPulling();
                }
            }

            //gravity scripit
            if (!climbWall && !wallgrab && !pullingRope)
            {
                speedVelocity.y += gravity * Time.deltaTime;
            }

            if (!climbWall)
            {
                playerController.Move(speedVelocity * Time.deltaTime);
            }

            //jumping
            if (Input.GetButtonDown("Jump"))
            {
                Jump(z, x);
            }

            //dashing
            if (Input.GetKeyDown(KeyCode.LeftControl) & !dashing & !dashcooldown)
            {
                Dash(z, x);
            }

            //while facing a climbable wall and pressing forwards you will climb upwards results in +y 
            if (climbWall)
            {
                climbing(z);
            }
        }
    }

    private void Dashingreset()
    {
        dashing = false;
    }
    
    private void Walljumpingreset()
    {
        walljumping = false;
    }

    private void Dashcooldown()
    {
        dashcooldown = false;
    }

    private void Dash(float z, float x)
    {
        dashing = true;
        dashcooldown = true;
        if (z < 1)
        {
            z = 1;
        }
        movement = transform.right * x *6f + transform.forward * z *6f;
        playerController.Move(movement * speedfactor * Time.deltaTime);
        Invoke("Dashingreset", 0.2f );
        Invoke("Dashcooldown", dashCooldownTimer);
    }

    private void Jump(float z, float x)
    {
        animator.SetBool("bottom", false);
        if (Input.GetButton("Jump") && isOnFloor)
        {
            speedVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            hasAirjump = true;
            animator.SetTrigger("Jump");
        }
        else if (climbWall)
        {
            speedVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            hasAirjump = true;
            animator.SetTrigger("Jump");
        }
        else if (walljump)
        {
            wallgrab = false;
            if (z <= 0)
            {
                z = 1;
            }
            walljumping = true;
            Invoke("Walljumpingreset", 1f);
            movement = transform.right * x *1f  + transform.forward * z *-1f;
            playerController.Move(movement * speedfactor * Time.deltaTime);
            speedVelocity.y = Mathf.Sqrt(jumpHeight/3.5f * -2f * gravity);
        }
        else if (hasAirjump && !pullingRope)
        {
            speedVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            hasAirjump = false;
        }
    }

    private void climbing(float z)
    {
        hasAirjump = true;
        
        if (z <= 0)
        {
            speedVelocity.y = -2;
        }
        else
        {
            speedVelocity.y = 4;
        }
        playerController.Move((movement + speedVelocity ) * 1 * Time.deltaTime);
    }

    private void GrabWall()
    {
        wallgrab = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "trampolin": 
                speedVelocity.y = Mathf.Sqrt(jumpHeight*3 * -2f * gravity);
                hasAirjump = true;
                break;
        }
    }

    public void RopePulling(Vector3 pullpos)
    {
        pullingRope = true;
        objectPos = pullpos;
        //Vector3 directionVector =  (PullingObject.transform.position - floorCheck.transform.position).normalized;
        //playerController.Move(directionVector * 20 * Time.deltaTime);
        movement =  (objectPos - floorCheck.transform.position).normalized;
    }

    public void StopPulling()
    {
        pullingRope = false;
    }

}