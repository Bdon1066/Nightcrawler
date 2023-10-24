using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PlayerController pc;
    private Animator pcAnimator;
    // Start is called before the first frame update
    void Start()
    {
        pcAnimator= pc.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (pcAnimator.GetBool("isAttacking"))
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Destroy(collider.gameObject);
                Debug.Log("KILL");
            }
        }
            
    }
}

