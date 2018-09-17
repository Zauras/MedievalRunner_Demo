using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour {

    private Player player;

    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private float currentHP, maxHP;

    void Start()
    {
        //Player hp info:
        player = Player.instance;
        maxHP = currentHP = player.GetHP();

        //Slider ref:
        healthBar.maxValue = maxHP;
        healthBar.minValue = 0f;
        healthBar.value = healthBar.maxValue;
    }

    void Update()
    {
        currentHP = player.GetHP(); //update hp info
        SlideHP();
    }

    void SlideHP()
    {
        if (currentHP >= 0)
        {
           healthBar.value = currentHP;
        }
    }

    float CalculateHP()
    {   
        // Get % of hp
        return currentHP / maxHP;
    }

}

