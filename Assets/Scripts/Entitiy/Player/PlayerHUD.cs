using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    private Stats playerStats;
    private Stamina playerStamina;
    public GameObject playerHUD;
    public Slider healthBar;
    public Slider staminaBar;


    // Start is called before the first frame update
    void Start()
    {

        playerStats = GetComponent<Stats>();
        playerStamina = GetComponent<Stamina>();
        healthBar.maxValue = playerStats.MaxHealth;
        staminaBar.maxValue = playerStamina.MaxStamina;
        playerHUD.SetActive(true);
        
    }

    // wastefull atm, find way to make this occur when it needs to 
    void Update()
    {
        healthBar.value = playerStats.Health;
        staminaBar.value = playerStamina.currentStamina;
    }
}
