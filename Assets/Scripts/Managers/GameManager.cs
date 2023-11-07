using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}

