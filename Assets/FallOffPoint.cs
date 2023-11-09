using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerController>().Death();
            print("fall off");
        }
    }
}
