using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI teleportStaminaText;
    [SerializeField] private Slider teleportStaminaSlider;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI timerText;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI gameEndTimeText;
    [SerializeField] private TextMeshProUGUI noOfDeathsText;
    [Space(10)]
    [SerializeField] private GameObject objective;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject timer;
    private void Start()
    {
        teleportStaminaSlider.maxValue = 200; //hard coding this kinda sucks but cant be arsed wiring it up to the PC rn
    }

    public void UpdateHUD(float health, float teleportStamina)
    {
        health = Mathf.Round(health);
        teleportStamina = Mathf.Round(teleportStamina);
        healthText.text = ("HP " + health.ToString());
        healthSlider.value = health;
        teleportStaminaText.text = ("AP " + teleportStamina.ToString());
        teleportStaminaSlider.value = teleportStamina;
    }
    public void UpdateTimer(float time)
    {
        timerText.text = ("Time ") + time.ToString("0.00");
    }
    

    public void HideObjective()
    {
        objective.gameObject.SetActive(false);
    }
    public void GameOverScreen(int enemiesKilled, float gameEndTime,int noOfDeaths)
    {
        gameOver.gameObject.SetActive(true);
        HUD.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        enemiesKilledText.text = ("Enemies Killed " + enemiesKilled.ToString());
        gameEndTimeText.text = ("Time " + gameEndTime.ToString("0.00"));
        noOfDeathsText.text = ("Deaths " + noOfDeaths.ToString());

    }
}

