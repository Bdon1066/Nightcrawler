using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.LowLevel;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refrences")]
    public CharacterController controller;
    public Transform transform;
    public Transform camera;
    public Transform  groundCheckPos;
    public LayerMask groundLayer;
    public LayerMask teleportThruLayer;

    [Header("Movement")]
    public float speed = 12f;
    public float gravityScale = 5f;
    public float jumpSpeed = 10f;
    private float ySpeed;

    [Header("Teleporting")]
    [SerializeField] private float teleportDistance;
    [SerializeField] private float teleportSpeed;
    private float teleportTime;
    private Vector3 teleportTarget;
    private bool isTeleporting;
    public LineRenderer teleportLineDraw;

    [Header("Camera")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private void Start()
    {
        teleportTime = teleportDistance / teleportSpeed;
    }
    void Update() //for detecting inputs
    {
        Jump();
        Teleport();

    }
    void FixedUpdate() //for calaculating movement
    {
        if (!isTeleporting)
        {
            Move();
        }
        else
        {
            TeleportMove(teleportTarget);   
        }
       
        Gravity();

    }

    private void Jump()
    {
       if (Input.GetButtonDown("Jump") && IsGrounded())
       {
            ySpeed = jumpSpeed;
       }
    }
    void Teleport()
    {
        // Camera cameraObject = camera.gameObject.GetComponent<Camera>();
        // Vector3 rayOrigin = cameraObject.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)); //for maybe making it camera based

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0)) //left click
        {
            teleportLineDraw.SetPosition(0, controller.transform.position);
            if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, teleportDistance, ~ teleportThruLayer))
            {
                teleportLineDraw.SetPosition(1, hit.point);
                teleportTarget = hit.point;
                isTeleporting = true;
            }
            else
            {
                teleportLineDraw.SetPosition(1, controller.transform.position + (controller.transform.forward * teleportDistance));
                teleportTarget = controller.transform.forward * teleportDistance;
                isTeleporting = true;
            }
        }
    }
   
    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = Vector3.zero;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;
    
        }
        moveDir.y = ySpeed;
       
        controller.Move(moveDir * Time.deltaTime);

    }
    void TeleportMove(Vector3 target)
    {
      
        Vector3 moveDir = Vector3.zero;
        Vector3 direction = new Vector3(target.x, 0f, target.z).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * teleportSpeed;

        }
        gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        controller.Move(moveDir * Time.deltaTime);
        Invoke("StopTeleport", teleportTime);
       
    }
    void StopTeleport()
    {
        isTeleporting = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    private void Gravity()
    {
        ySpeed += Physics.gravity.y * gravityScale * Time.deltaTime;
    }



    private bool IsGrounded()
    {
        if(Physics.CheckSphere(groundCheckPos.position, 0.4f, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
       
    }

}
