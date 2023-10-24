using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.LowLevel;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    [Header("Refrences")]
    public CharacterController controller;
    public GameManager gameManager;
    public Animator animator;
    public Transform transform;
    public Transform camera;
    public Transform  groundCheckPos;
    public GameObject transparentPlayer;
    public LayerMask groundLayer;
    public LayerMask teleportThruLayer;
    public LayerMask invisibleLayer;

    [Header("Movement")]
    public float speed = 12f;
    public float sprintMultiplier = 1.2f;
    public float gravityScale = 5f;
    public float jumpSpeed = 10f;
    private float ySpeed;
    private bool isSprinting;
    private bool enableGravity = true;
    private bool enableMovement = true;

    [Header("Teleporting")]
    [SerializeField] private float startingTeleportDistance;
                     private float teleportDistance;
    [SerializeField] private float teleportSpeed;
    private float teleportTime;
    private Vector3 teleportTarget;
    private bool isTeleporting;
    [HideInInspector ]public bool isInvisible;
    public LineRenderer teleportLineDraw;
    [Header("Combat")]
    public Transform combatHand;

    [Header("Camera")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private void Start()
    {
        teleportDistance = startingTeleportDistance;
        teleportLineDraw.enabled = false;
    }
    void Update() //for detecting inputs
    {
        Jump();
        TeleportInput();
        Attack();
        Sprint();

       
    }
    void FixedUpdate() //for calaculating movement
    {
        if (!isTeleporting)
        {
            Move();
        }
        else
        {
            TeleportMovement();   
        }
       
        Gravity();

    }
    private void Gravity()
    {
        if (enableGravity){
            ySpeed += Physics.gravity.y * gravityScale * Time.deltaTime;
        }
    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            ySpeed = jumpSpeed;
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
        if (enableMovement && !isSprinting) { controller.Move(moveDir * Time.deltaTime); }
        else if (enableMovement && isSprinting) { controller.Move((moveDir * sprintMultiplier) * Time.deltaTime); }


    }
    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            print("is sprinting");
        }
        else
        {
            isSprinting = false;
        }
    }


    void TeleportInput() 
    {
        //When player holds down left click || TODO:  maybe Add so they have to hold down for a certain amount of time before telporting activates
        if (Input.GetMouseButton(1))
        {
            FreezeMovement();
            TeleportGraphics();
            gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
            isInvisible = true;
        }
        //When player lets go of left click 
        if (Input.GetMouseButtonUp(1))
        {
            Teleport();
            
        }
    }
    void TeleportGraphics() //handles graphical aspect of teleporting
    {
       
        GameObject transparentWall;

        //draws line of where teleport will go || TODO add a little silloute thing where the player will end up
        teleportLineDraw.enabled = true;
        teleportLineDraw.SetPosition(0, controller.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, teleportDistance, ~teleportThruLayer))
        {
            teleportLineDraw.SetPosition(1, hit.point);
            
        }
        else
        {
            teleportLineDraw.SetPosition(1, controller.transform.position + (controller.transform.forward * teleportDistance));
            
        }

       //Make the wall we are looking to teleport thru transparent
        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, teleportDistance, teleportThruLayer)) 
        {
           
            transparentWall = hit.collider.gameObject;

            transparentWall.GetComponent<TransparentWall>().Transparent();
        }
      


    }
    void Teleport()
    {
        RaycastHit hit;
       
        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, teleportDistance, ~teleportThruLayer))
        {
            teleportTarget = hit.point;
            teleportDistance = Vector3.Distance(controller.transform.forward, hit.point);
            isTeleporting = true;
        }
        else
        {
            teleportTarget = controller.transform.forward * teleportDistance;
            isTeleporting = true;
        }
    }
    
    void TeleportMovement() //when isTeleporting True
    {
        teleportTime = teleportDistance / teleportSpeed;

        Vector3 moveDir = Vector3.zero;
        Vector3 direction = new Vector3(teleportTarget.x, 0f, teleportTarget.z).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * teleportSpeed;

        }
       

        controller.Move(moveDir * Time.deltaTime);
        
        Invoke("StopTeleport", teleportTime - 0.01f);
       
    }
  
    
    void StopTeleport()
    {

        controller.Move(Vector3.zero);
        isTeleporting = false;
        isInvisible = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        UnFreezeMovement();
        teleportLineDraw.enabled = false;


    }
    void FreezeMovement()
    {
        enableGravity = false;
        enableMovement = false;
       
    }
    void UnFreezeMovement()
    {
        enableGravity = true;
        enableMovement = true;
    }
    void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
       
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
