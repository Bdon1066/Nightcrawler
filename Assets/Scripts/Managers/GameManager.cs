using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Abertay.Analytics;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    private bool gameOver;
    //STATS 
    [HideInInspector] public float gameTime;
    [HideInInspector] public int enemiesKilled;
    [HideInInspector] public int noOfDeaths;
    [HideInInspector] public int noOfTeleports;
    [HideInInspector] public float overallDamageTaken;
    [HideInInspector] public float timeTillfirstTeleport = 0;
    [HideInInspector] public string deathLocation;
    [HideInInspector] public bool wonGame;
    [SerializeField] GameObject enemies;
    int totalEnemies =1;
    [SerializeField] PlayerController pc;


    void Start()
    {
   
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        StartCoroutine(Intro());
        foreach (Transform child in enemies.transform)
        {
            totalEnemies++;
        }
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(5f);
        uiManager.HideObjective();
    }

    public void EndGame() //when player kills president
    {
        StartCoroutine(Outro());
    }
    public void ResetLevel() //when player dies
    {
        gameTime = 0;
        enemiesKilled = 0;
        foreach (Transform child in enemies.transform)
        {
            child.gameObject.SetActive(true);
        }

    }
    IEnumerator Outro()
    {
        wonGame = true;
        uiManager.GameOverScreen(enemiesKilled, totalEnemies,gameTime, noOfDeaths);
        Analytics(null);

        gameOver = true;
        yield return new WaitForSeconds(5f);
        Application.Quit();
        Debug.Log("Quit");
    }
    public void Analytics(string causeOfDeath)
    {
        
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("wonGame", wonGame);
        if (!wonGame) { data.Add("causeOfDeath", causeOfDeath); data.Add("deathLocation", deathLocation); }

        data.Add("enemiesKilled", enemiesKilled);
        data.Add("gameTime", Mathf.Round(gameTime * 100.0f) * 0.01f);
        data.Add("noOfDeaths", noOfDeaths);
        data.Add("overallDamageTaken", overallDamageTaken);

        data.Add("noOfTeleports", noOfTeleports / 5);
        data.Add("timeTillfirstTeleport", Mathf.Round(timeTillfirstTeleport * 100.0f) * 0.01f);
        data.Add("avgTimeSpentTeleporting", Mathf.Round(pc.AvgTimeSpentTeleporting() * 100.0f) * 0.01f);
       
        AnalyticsManager.SendCustomEvent("runEnded", data);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit");
        }
        if (!gameOver)
        {
            gameTime += Time.deltaTime;
            uiManager.UpdateTimer(gameTime);
        }

       
    }
    public void LogFirstTeleportInput(float time)
    {
        timeTillfirstTeleport = time;
    }
    public Transform FindNearestEnemy(Transform playerLocation)
    {
 
        float minDistance = 100;
        Transform nearestEnemy =  enemies.transform.GetChild(0);
        foreach (Transform child in enemies.transform)
        {
            if (child.gameObject.activeInHierarchy == true)
            {
                var distance = Vector3.Distance(playerLocation.position, child.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = child;
                }
            }
            
           
        }
      //  print(nearestEnemy.gameObject);
        return nearestEnemy;
        
    }
}



