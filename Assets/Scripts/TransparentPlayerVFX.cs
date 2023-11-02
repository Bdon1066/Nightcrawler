using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentPlayerVFX : MonoBehaviour
{
    private bool shouldBeEnabled = false;

    public void Enable(Vector3 position, Quaternion rotation)
    {
        gameObject.SetActive(true);
        shouldBeEnabled = true;
        transform.position = position;
        transform.rotation = rotation;
    }
    public void Disable()
    {
        gameObject.SetActive(false);

    }

}
