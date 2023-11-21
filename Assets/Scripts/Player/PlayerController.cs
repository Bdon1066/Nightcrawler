using Abertay.Analytics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private CharacterController controller;
    [SerializeField] public GameManager gameManager;
    [SerializeField] private UIManager uIManager;
    [Space(10)]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerGraphics playerGraphics;
    [SerializeField] private Transform camera;
    [SerializeField] private Transform  groundCheckPos;
    [SerializeField] private TransparentPlayerVFX transparentPlayerVFX;
    [SerializeField] private Transform playerSpawn;
    [Space(10)]
     public LayerMask groundLayer;
     public LayerMask teleportThruLayer;
     public LayerMask playerAttackCollider;

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
    [Space(10)]
    [SerializeField] private float startingTeleportStamina = 100f;
    [SerializeField] private float teleportCost = 10f;
    [SerializeField] private float teleportStaminaRegen = 1f;
    [SerializeField] private float staminaRegenDelay = 0.5f;
     private float teleportStamina;
     private bool regenStamina = true;
    [HideInInspector] public bool isInvisible;
     private float teleportDistance;
     private float teleportTime;
     private Vector3 teleportTarget;
     private bool isTeleporting;
     private bool hasTeleportedBefore;
     private float timeSpentTeleporting;
     private List<float> timeSpentTeleportingValues = new List<float>();
    [Space(10)]
     public LineRenderer teleportLineDraw;
    [SerializeField] private TeleportHeatmap heatmap;
     private bool spawnTransparentPlayer;

    [Header("Combat")]
    [SerializeField] private Transform combatHand;
    [SerializeField] private float startingHealth;
    [SerializeField] private float healthRegen;
    [SerializeField] private float healthRegenDelay;
     private float health;
    [Header("Analytics")]
    [SerializeField] GameObject roomTriggers;

    //CAMERA
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    //HEATMAP SHENINEGANS
    bool heatmapStartTrigger;
    bool heatmapEndTrigger;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        health = startingHealth;
        teleportStamina = startingTeleportStamina;
        uIManager.UpdateHUD(health, teleportStamina);

        teleportDistance = startingTeleportDistance;
        hasTeleportedBefore = false;
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
        HealthRegen();
        
       if (isInvisible)
        {
            timeSpentTeleporting += Time.deltaTime;

        }

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
            if (!hasTeleportedBefore)
            {
                hasTeleportedBefore = true;
                gameManager.LogFirstTeleportInput(gameManager.gameTime);
                
            }
            FreezeMovement();
            TeleportGraphics();
            gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
           
            isInvisible = true;
           
        }
        //When player lets go of left click 
        if (Input.GetMouseButtonUp(1) && teleportStamina >= teleportCost)
        {
            teleportStamina -= teleportCost;   
            Teleport();
        }

    }
    void TeleportGraphics() //handles graphical aspect of teleporting
    {
        playerGraphics.TransparentGraphics(); //set player model to be transparent

        GameObject transparentWall;

        //draws line of where teleport will go || TODO add a little silloute thing where the player will end up
        teleportLineDraw.enabled = true;
        teleportLineDraw.SetPosition(0, controller.transform.position);
        //heatmap.HeatmapStart(controller.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, teleportDistance, ~teleportThruLayer))
        {
            teleportLineDraw.SetPosition(1, hit.point);
        }
        else
        {
            teleportLineDraw.SetPosition(1, controller.transform.position + (controller.transform.forward * teleportDistance));
            transparentPlayerVFX.Enable(teleportLineDraw.GetPosition(1), transform.rotation);
            
        }

       //Make the wall we are looking to teleport thru transparent
        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, teleportDistance, ~playerAttackCollider)) 
        {
            transparentWall = hit.collider.gameObject;
            if (transparentWall.gameObject.CompareTag("Teleportable"))
            {
                transparentWall.GetComponent<TransparentWall>().SignalTransparent();
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
           // heatmap.HeatmapEnd(controller.transform.forward * teleportDistance);
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
        gameManager.noOfTeleports++;
        print(gameManager.noOfTeleports);
        controller.Move(Vector3.zero);
        isTeleporting = false;
       
        timeSpentTeleportingValues.Add(timeSpentTeleporting);
        isInvisible = false;
        playerGraphics.ResetGraphics();
        transparentPlayerVFX.Disable();
       
        gameObject.layer = LayerMask.NameToLayer("Player");
        UnFreezeMovement();
        teleportLineDraw.enabled = false;

        StartCoroutine("StaminaRegenDelay");
    }
   



    void Stamina()
    {
        uIManager.UpdateHUD(health, teleportStamina);

        if (isInvisible)
        {
            teleportStamina -= Time.deltaTime * teleportStaminaRegen;
            
        }
        else if (teleportStamina < startingTeleportStamina && regenStamina)
        {
            teleportStamina += Time.deltaTime * teleportStaminaRegen;
           
        }
        else if (teleportStamina >= startingTeleportStamina)
        {
            teleportStamina = startingTeleportStamina;
           
        }

        if (teleportStamina <= 0)
        {
            teleportStamina = 0;
            StopTeleport();
            
        }

        
    }
   
    void HealthRegen() 
    {
        uIManager.UpdateHUD(health, teleportStamina);

        if (isInvisible )
        {
            health += Time.deltaTime * healthRegen;
        }
        if (health >= startingHealth)
        {
            health = startingHealth;
        }

       
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    public void TakeDamage(float damageAmount, string causeOfDamage)
    {
        gameManager.overallDamageTaken += damageAmount;

        health = health - damageAmount;
        uIManager.UpdateHUD(health,teleportStamina);
      //  gameManager.deathLocation = FindRoomOnDeath();
        if (health <= 0)
        {
            health = 0;
           
            Death(causeOfDamage);
        }
    }
    public void Death(string deathCause)
    {
        gameManager.noOfDeaths++;
        DeathAnalytics(deathCause);
        gameManager.ResetLevel();
        Respawn();
    }
   
    private void Respawn()
    {
        Initialize();
    }
    IEnumerator StaminaRegenDelay()
    {
        regenStamina = false;
        yield return new WaitForSeconds(staminaRegenDelay);
        regenStamina = true;
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
    private void DeathAnalytics(string deathCause)
    {
        gameManager.Analytics(deathCause);

        AnalyticsManager.LogHeatmapEvent("PlayerDeath", controller.transform.position, Color.red);
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
    public float AvgTimeSpentTeleporting()
    {
        var fixedNoTeleports = gameManager.noOfTeleports / 5;
       return timeSpentTeleporting / fixedNoTeleports;
    }
    public string FindRoomOnDeath() //fucking does not work 
    {
        
        foreach (Transform child in roomTriggers.transform)
        {
          
           var roomTrigger = child.gameObject.GetComponent<RoomTrigger>();
           if (roomTrigger.playerIsInRoom)
           {
                print(child.name);
                return child.name;
           }

        }
        return null;

       
    }

}
