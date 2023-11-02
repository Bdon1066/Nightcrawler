using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PlayerController pc;
    private Animator pcAnimator;
    Vector3 previousPosition;
    public LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        pcAnimator = pc.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (previousPosition == Vector3.zero)
        //{
        //    previousPosition = transform.position;
        //}
        //else
        //{
        //    var direction = (previousPosition - transform.position).normalized;
        //    var distance = Vector3.Distance(previousPosition, transform.position);
        //    RaycastHit hit;
        //    if (Physics.Raycast(transform.position, direction, out hit, distance, enemyLayer))
        //    {
        //        print("hit somehing");
        //        if (hit.collider.gameObject.CompareTag("Enemy"))
        //        {
        //            Destroy(hit.collider.gameObject);
        //        }
        //        if (hit.collider.gameObject.CompareTag("President"))
        //        {
        //            Destroy(hit.collider.gameObject);
        //            pc.uIManager.GameOverScreen();
        //        }
        //    }

        //}
        //previousPosition = Vector3.zero;

    }


    private void OnTriggerEnter(Collider collider)
    {
        {
            if (pcAnimator.GetBool("isAttacking"))
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    Destroy(collider.gameObject);
                }
                if (collider.gameObject.CompareTag("President"))
                {
                    Destroy(collider.gameObject);
                    pc.uIManager.GameOverScreen();
                }
            }

        }
    }
}

