using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    private bool gameOver;
    private float gameTime = 0;
    public int enemiesKilled = 0;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(5f);
        uiManager.HideObjective();
    }

    public void EndGame()
    {
        StartCoroutine(Outro());
    }
    IEnumerator Outro()
    {
        uiManager.GameOverScreen(enemiesKilled,gameTime);
        gameOver = true;
        yield return new WaitForSeconds(5f);
        Application.Quit();
        Debug.Log("Quit");
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

