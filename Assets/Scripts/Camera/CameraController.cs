using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera thirdPersonCam;
    [SerializeField] Camera overShoulderCam;


    void Start()
    {
        thirdPersonCam.enabled = true;
        overShoulderCam.enabled = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            thirdPersonCam.enabled = !thirdPersonCam.enabled;
            overShoulderCam.enabled = !overShoulderCam.enabled;
        }
  
    }
}

