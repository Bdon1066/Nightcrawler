using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    private Vector2 move;
    public float speed = 6f;
    


    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("move");
        move = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("jump");
    }

    void Update()
    {
       
        Vector3 direction = new Vector3(move.x, 0f, move.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
    
}
