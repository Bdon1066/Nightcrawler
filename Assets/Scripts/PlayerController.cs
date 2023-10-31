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
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerGraphics playerGraphics;
    [SerializeField] private Transform camera;
    [SerializeField] private Transform  groundCheckPos;
    [SerializeField] private GameObject transparentPlayer;
    [SerializeField] private Transform playerSpawn;
                     public LayerMask groundLayer;
                     public LayerMask teleportThruLayer;

    [Header("Movement")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float gravityScale = 9f;
    [SerializeField] private float jumpSpeed = 40f;
                     private float ySpeed;
                     private bool isSprinting;
                     private bool enableGravity = true;
                     private bool enableMovement = true;

    [Header("Teleporting")]
    [SerializeField] private float startingTeleportDistance;  
    [SerializeField] private float teleportSpeed;
    [SerializeField] private float startingTeleportStamina = 100f;
    [SerializeField] private float teleportStaminaDPS = 1f;
                     private float teleportStamina;
    [HideInInspector] public bool isInvisible;
                     private float teleportDistance;
                     private float teleportTime;
                     private Vector3 teleportTarget;
                     private bool isTeleporting;
                     public LineRenderer teleportLineDraw;

    [Header("Combat")]
    [SerializeField] private Transform combatHand;
    [SerializeField] private float startingHealth;
                     private float health;

    //CAMERA
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        health = startingHealth;
        teleportStamina = startingTeleportStamina;
        uIManager.UpdateUI(health, teleportStamina);

        teleportDistance = startingTeleportDistance;
        teleportLineDraw.enabled = false;

        controller.enabled = false;
        transform.position = playerSpawn.position;
        transform.rotation = playerSpawn.rotation;
        controller.enabled = true;
    }
    void Update() //for detecting inputs
    {
        Jump();
        TeleportInput();
        Attack();
        Sprint();
        Stamina();

       
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
        if (Input.GetMouseButtonUp(1) && teleportStamina > 10)
        {
            teleportStamina -= 10;
            Teleport();
            
        }

        // TO DO ADD UH YOU CANT TELEPORT UNLESS YOU HAVE ENOYGH STAMINAAAAAAAHSD98UWQ0D80QWUIDS[QWDI[WQ90[DWQ[D0-I[EQCIXHE87CY9-EWUC;
    }
    void TeleportGraphics() //handles graphical aspect of teleporting
    {
        playerGraphics.TransparentGraphics(); //set player model to be transparent

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

        GameObject transparentWall;
        //Make the wall we are looking to teleport thru transparent
        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, teleportDistance, teleportThruLayer)) 
        {
            transparentWall = hit.collider.gameObject;

            if (transparentWall.GetComponent<TransparentWall>() != null)
            {
                transparentWall.GetComponent<TransparentWall>().Transparent();
            }
     
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
        playerGraphics.ResetGraphics();

        gameObject.layer = LayerMask.NameToLayer("Player");
        UnFreezeMovement();
        teleportLineDraw.enabled = false;
    }
    void Stamina()
    {
        uIManager.UpdateUI(health, teleportStamina);
        if (isInvisible)
        {
            teleportStamina -= Time.deltaTime * teleportStaminaDPS;
            
        }
        else if (teleportStamina < 100)
        {
            teleportStamina += Time.deltaTime * teleportStaminaDPS;
           
        }
        else if (teleportStamina >= 100)
        {
            teleportStamina = 100;
           
        }

        if (teleportStamina <= 0)
        {
            teleportStamina = 0;
            StopTeleport();
        }

        
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
    public void TakeDamage(float damageAmount)
    {
        health = health - damageAmount;
        uIManager.UpdateUI(health,teleportStamina);

        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }
    private void Death()
    {
        Respawn();
    }
    private void Respawn()
    {
        Initialize();
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

    private void Gravity()
    {
        if (enableGravity)
        {
            ySpeed += Physics.gravity.y * gravityScale * Time.deltaTime;
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
