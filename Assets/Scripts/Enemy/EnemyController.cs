using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyController : MonoBehaviour
{
    [Header("Refrences")]
    public CharacterController controller;
    public Transform transform;
    public GameObject gun;
    public Transform playerLocation;
    private PlayerController player;
    public LayerMask playerLayer;

    [Header("Movement")]
    public float speed = 12f;
    private float ySpeed;
    public float gravityScale = 5f;
    private bool enableGravity = true;
    private bool enableMovement = true;
    [Header("Combat")]
    public float sightDistance;
    public float shootInterval;
    private float shootTimer;

    private void Start()
    {
        player = playerLocation.gameObject.GetComponent<PlayerController>();
    }
    void Update() //for detecting inputs
    {
        Attack();
    }
    void FixedUpdate() //for calaculating movement
    {
        PlayerSearch();
        Gravity();
        Move();
       
    }
    void PlayerSearch()
    {
        RaycastHit hit;
        Vector3 direction = (playerLocation.position - this.transform.position).normalized;
        Vector3 eyePosition = (controller.transform.position) + new Vector3(0, 1, 0);
       if( Physics.Raycast(eyePosition, direction, out hit, sightDistance, ~playerLayer))
       {
            if (hit.collider.gameObject.CompareTag("Player") && !player.isInvisible)
            {
               // print(this.gameObject.name + " CAN see player");
                AttackState();
            }
            else
            {
                IdleState();
            }

        }

    }
    void AttackState()
    {
        var playerPos = playerLocation.position;
        gun.transform.LookAt(playerPos);

        Vector3 yClampedPos = new Vector3(playerPos.x, this.transform.position.y, playerPos.z);
        transform.LookAt(yClampedPos);

        var gunScript = gun.GetComponent<Gun>();
        
        if (shootTimer >= shootInterval)
        {
            
            gunScript.FireBullet(playerLocation);
            shootTimer = 0;
        }
        else
        {
            shootTimer += Time.deltaTime;
        }

       
    }
    void IdleState()
    {
        transform.RotateAround(transform.position, Vector3.up, 180 * Time.deltaTime);
    }
    private void Gravity()
    {
        if (enableGravity)
        {
            ySpeed += Physics.gravity.y * gravityScale * Time.deltaTime;
        }
    }
    private void Move()
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.y = ySpeed;
        if (enableMovement) { controller.Move(moveDir * Time.deltaTime); }
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
        
    }

}
