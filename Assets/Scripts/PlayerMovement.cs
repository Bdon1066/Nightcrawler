using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refrences")]
    public CharacterController controller;
    public Transform transform;
    public Transform camera;
    public Transform  groundCheckPos;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float speed = 6f;
    public float gravityScale = 1f;
    public float jumpSpeed = 10f;
    private float ySpeed;
    [Header("Teleporting")]
    public float teleportRange;

    [Header("Camera")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    void Update()
    {
        Jump();
        Teleport();
    }
    void FixedUpdate()
    {
        Move();
        Gravity();
        
    }

    private void Jump()
    {
       if (Input.GetButtonDown("Jump") && IsGrounded())
       {
            ySpeed = jumpSpeed;
       }
    }
    private void Gravity()
    {
        ySpeed += Physics.gravity.y * gravityScale * Time.deltaTime;
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
    void Teleport()
    {
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
           

            if (Physics.Raycast(this.transform.position, Vector3.forward, out hit, teleportRange)){
                Debug.Log("We be teleportin");

               

            }
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
