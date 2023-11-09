using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform lookAtPosition;
    [SerializeField] float cameraDistance;
    [Space(10)]
    [SerializeField] float maxVerticalAngle;
    [SerializeField] float minVerticalAngle;
    [Space(10)]
    [SerializeField] float horizontalRotationSpeed;
    [SerializeField] float verticalRotationSpeed;
    [Space(10)]
    [SerializeField] Vector2 cameraOffset;
    [Space(10)]
    [SerializeField] bool invertCameraControls;

    float yRotation;
    float xRotation;

 
    void Update()
    {

        yRotation += Input.GetAxis("Mouse X") * horizontalRotationSpeed;

        if (invertCameraControls) { xRotation -= Input.GetAxis("Mouse Y") * verticalRotationSpeed; } //inverts vertical movement of camera
        else { xRotation += Input.GetAxis("Mouse Y") * verticalRotationSpeed; }

        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        var rotation = Quaternion.Euler(xRotation, yRotation, 0);
        var distance = new Vector3(0, 0, cameraDistance);
        var offset = lookAtPosition.position + new Vector3(cameraOffset.x, cameraOffset.y);

        transform.position = offset - rotation * distance;
        transform.rotation = rotation;

    }
}


