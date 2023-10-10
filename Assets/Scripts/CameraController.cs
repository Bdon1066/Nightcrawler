using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform lookAtPosition;
    [Header("Settings")]

    [SerializeField] float cameraDistance;

    [SerializeField] float maxVerticalAngle;
    [SerializeField] float minVerticalAngle;

    [SerializeField] float horizontalRotationSpeed;
    [SerializeField] float verticalRotationSpeed;

    [SerializeField] Vector2 cameraOffset;

    float yRotation;
    float xRotation;


    void Update()
    {
        yRotation += Input.GetAxis("Mouse X") * horizontalRotationSpeed;
        xRotation += Input.GetAxis("Mouse Y") * verticalRotationSpeed;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        var rotation = Quaternion.Euler(xRotation, yRotation, 0);
        var distance = new Vector3(0, 0, cameraDistance);
        var offset = lookAtPosition.position + new Vector3(cameraOffset.x, cameraOffset.y);

        transform.position = offset - rotation * distance;
        transform.rotation = rotation;  
    }
}

