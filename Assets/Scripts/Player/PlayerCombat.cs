using Cinemachine.Utility;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerCombat : MonoBehaviour
{
    public PlayerController controllerScript;
    public GameManager gameManager;
    public GameObject playerFist;
    private Animator pcAnimator;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public float enemyAngle;
    Vector3 yClampedEnemyPos;
    bool enemyInRange;
    [Space(10)]
    public bool autoAim;
    public float autoAimRange = 5f;

    // Start is called before the first frame update
    void Start()
    {
        pcAnimator = controllerScript.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (isAttacking && enemyInRange && autoAim)
        {
            transform.LookAt(yClampedEnemyPos);
        }
    }

   public void IsAttacking()
   {
        isAttacking = true;
    }
   public void IsNotAttacking()
   {
        isAttacking = false;
   }
    void Attack()
    {
        pcAnimator.SetTrigger("Attack");

        //AUTOAIM
        var enemyLocation = gameManager.FindNearestEnemy(transform);
        print(Vector3.Distance((transform.position), enemyLocation.position));

        if (Vector3.Distance((transform.position),enemyLocation.position) < autoAimRange)
        {
            enemyInRange = true;

            if (enemyLocation.gameObject.CompareTag("Enemy") && (enemyLocation.gameObject.activeInHierarchy == true))
            {
                var enemyPos = enemyLocation.position;

                yClampedEnemyPos = new Vector3(enemyPos.x, this.transform.position.y, enemyPos.z);

            }
        }
        else
        {
            enemyInRange = false;
        }
        
    }
    public void DamageEnemy(GameObject enemy)
   {
        gameManager.enemiesKilled += 1;
        controllerScript.IncreaseStamina(50);

        if (enemy.CompareTag("President"))
        {
            gameManager.EndGame();
            Destroy(enemy.gameObject);
        }
        if (enemy.GetComponent<EnemyController>() != null)
        {
            enemy.GetComponent<EnemyController>().Death();
        }
        

    }
    





}

