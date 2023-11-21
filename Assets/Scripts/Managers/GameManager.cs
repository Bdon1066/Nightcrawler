using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Abertay.Analytics;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    private bool gameOver;
    [HideInInspector] public float gameTime;
    [HideInInspector] public int enemiesKilled;
    [HideInInspector] public int noOfDeaths;
    [HideInInspector] public int noOfTeleports;
    [HideInInspector] public int noOfShotTaken;
    [HideInInspector] public float overallDamageTaken;
    [SerializeField] GameObject enemies;


    void Start()
    {

        //  enemies.GetComponentInChildren<EnemyController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(Intro());

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
        foreach (Transform child in enemies.transform)
        {
            child.gameObject.SetActive(true);
        }

    }
    IEnumerator Outro()
    {
        uiManager.GameOverScreen(enemiesKilled,gameTime,noOfDeaths);
        Analytics();

        gameOver = true;
        yield return new WaitForSeconds(5f);
        Application.Quit();
        Debug.Log("Quit");
    }
    void Analytics()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("enemiesKilled", enemiesKilled);
        data.Add("gameTime", gameTime);
        data.Add("noOfDeaths", noOfDeaths);
        data.Add("noOfTeleports", noOfTeleports / 5);
        data.Add("noOfShotTaken", noOfShotTaken);
        data.Add("overallDamageTaken", overallDamageTaken);
        AnalyticsManager.SendCustomEvent("playerStats", data);
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
}

