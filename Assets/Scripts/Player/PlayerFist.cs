using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFist : MonoBehaviour
{
    public PlayerCombat combatScript;

    private void OnTriggerEnter(Collider collider)
    {
        if (combatScript.isAttacking)
        {
            if (collider.gameObject.CompareTag("Enemy") || (collider.gameObject.CompareTag("President")))
            {
                if (collider.gameObject != null)
                {
                    combatScript.DamageEnemy(collider.gameObject);
                }
               
            }

        }
    }
}
