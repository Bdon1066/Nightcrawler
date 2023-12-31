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
    public float shootIntervalMin;
    public float shootIntervalMax;
    private float shootInterval;
    private float shootTimer;
    private float spottedPlayerTimer;
    public float timeToSpotPlayer;

    private void Start()
    {
        player = playerLocation.gameObject.GetComponent<PlayerController>();
        shootInterval = Random.Range(shootIntervalMin, shootIntervalMax);

    }
    void Update() //for detecting inputs
    {
       
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
        if (Physics.Raycast(eyePosition, direction, out hit, sightDistance, ~playerLayer))
        {
            if (hit.collider.gameObject.CompareTag("Player") && !player.isInvisible)
            {
                var playerPos = playerLocation.position;
                gun.transform.LookAt(playerPos);

                Vector3 yClampedPos = new Vector3(playerPos.x, this.transform.position.y, playerPos.z);
                transform.LookAt(yClampedPos);

                // print(this.gameObject.name + " CAN see player");
                if (spottedPlayerTimer > timeToSpotPlayer)
                {
                    AttackState();
                    
                }
                else
                {
                    spottedPlayerTimer += Time.deltaTime;
                }
                
            }
            else
            {
                IdleState();
                spottedPlayerTimer = 0;
            }

        }
        else
        {
            IdleState();
            spottedPlayerTimer = 0;
        }

    }
    void AttackState()
    {
       

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
    public void Death() 
    {
        gameObject.SetActive(false);    
    }


}
