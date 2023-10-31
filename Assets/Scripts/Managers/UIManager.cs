using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI teleportStaminaText;

    public void UpdateUI(float health,float teleportStamina)
    {
        health = Mathf.Round(health);
        teleportStamina = Mathf.Round(teleportStamina);
        healthText.text = ("Health: " + health.ToString());
        teleportStaminaText.text = ("Teleport Stamina: " + teleportStamina.ToString());
    }
}
