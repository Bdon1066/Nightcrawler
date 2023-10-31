using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI teleportStaminaText;
    [SerializeField] private GameObject objective;
    [SerializeField] private GameObject gameOver;

    public void UpdateUI(float health,float teleportStamina)
    {
        health = Mathf.Round(health);
        teleportStamina = Mathf.Round(teleportStamina);
        healthText.text = ("Health: " + health.ToString());
        teleportStaminaText.text = ("Teleport Stamina: " + teleportStamina.ToString());
    }
    private void Start()
    {
        StartCoroutine("Intro");
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(5f);
        HideObjective();
    }
    void HideObjective()
    {
        objective.gameObject.SetActive(false);
    }
    public void GameOverScreen()
    {
        gameOver.gameObject.SetActive(true);
    }
}
