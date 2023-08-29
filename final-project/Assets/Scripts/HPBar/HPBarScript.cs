using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Gestisce i valori degli sliders della vita di Boss e player
public class HPBarScript : MonoBehaviour
{
    public Slider slider;
    public int currentHP;
    public int maxHP = 300;
    public bool isBoss = false;

    //Queste due funzioni settano vita massima e corrente quando vengono richiamate
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;    
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isBoss)
        {
            SetMaxHealth(Boss.maxHP);        // Assegnamento vita Boss
        }
        else
        {
            SetMaxHealth(HP_Player.maxHP);   // Assegnamento vita Player
        }
    }

    private void Update()     //Setto a ogni frame il valore corrente della vita nella barra
    { 
        if (isBoss)      
        {
            SetHealth(Boss.currentHP);
            currentHP = Boss.currentHP;
        }
        else
        {
            SetHealth(HP_Player.HP);
            currentHP = HP_Player.HP;
        }
    }
}
