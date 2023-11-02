using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverShoulderCam : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform target;
    [SerializeField] Transform player;
    [SerializeField] float cameraDistance;

    [SerializeField] float horizontalRotationSpeed;
    [SerializeField] float verticalRotationSpeed;

    [SerializeField] Vector2 cameraOffset;

    float yRotation;
    float xRotation;

    void Update()
    {
        yRotation += Input.GetAxis("Mouse X") * horizontalRotationSpeed;
        xRotation += Input.GetAxis("Mouse Y") * verticalRotationSpeed;

        var rotation = Quaternion.Euler(xRotation, yRotation, 0);
        var position = target.position;
        var distance = new Vector3(0, 0, cameraDistance);

        transform.position = position - distance;
       
        transform.forward = player.forward;
    }
}
