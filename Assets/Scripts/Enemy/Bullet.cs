using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {  
            //Debug.Log("Shot Player");
            collider.gameObject.GetComponent<PlayerController>().TakeDamage(25);
            Destroy(this.gameObject);
        }
    }
}
