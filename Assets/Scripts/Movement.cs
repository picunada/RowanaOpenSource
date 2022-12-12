using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    public static bool jumping;

    public static bool isSprinting = false;
    public float sprintMultiplier;
    

    public float crouchMultiplier;
    public static bool isCrouching = false;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dbbKey = KeyCode.K;
    public KeyCode dbKey = KeyCode.L;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public static bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        MyInput();
        SpeedControl();

    }


    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded && !isCrouching)
        {
            
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKey(dbbKey))
        {
            Debug.Log(grounded);

        }

        if (Input.GetKey(dbKey))
        {
            Debug.Log(readyToJump);

        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

    }

    private void MovePlayer()
    {
        
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded && isSprinting)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * sprintMultiplier, ForceMode.Force);

        else if (grounded && isCrouching)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * crouchMultiplier, ForceMode.Force);

        else if (grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);

        else if(!grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        Vector3 limitedVel = flatVel.normalized * moveSpeed;

        if (flatVel.magnitude > moveSpeed)
        {
        if (isSprinting && !isCrouching)
            {
                limitedVel *= sprintMultiplier;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            } else if (isCrouching && !isSprinting)
            {
                limitedVel *= crouchMultiplier;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            } else 
            {
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }
   

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        jumping = true;
    }


    private void ResetJump()
    {
        readyToJump = true;
        jumping = false;
    }
}


