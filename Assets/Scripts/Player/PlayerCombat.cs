using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PlayerController controllerScript;
    public GameManager gameManager;
    public GameObject playerFist;
    private Animator pcAnimator;
    public LayerMask enemyLayer;
    [HideInInspector] public bool isAttacking;

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
            pcAnimator.SetTrigger("Attack");
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

   public void DamageEnemy(GameObject enemy)
   {
        if (enemy.CompareTag("President"))
        {
            gameManager.EndGame();
            Destroy(enemy.gameObject);
        }
        enemy.GetComponent<EnemyController>().Death();
        gameManager.enemiesKilled += 1;

   }



}

