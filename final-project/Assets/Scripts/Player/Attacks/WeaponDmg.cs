using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDmg : MonoBehaviour
{
    public Animator animatorPlayer;       // Riferimento all'animator del player
    public bool isBoss = false;           // Controllo per verificare se sto combattendo il boss finale

    // Danno
    private int damageAmount;             // Variabile che conterrà il danno arrecato
    private int lightDamage;              // Variabile che conterrà il danno dell'attacco leggero dell'arma
    private int heavyDamage;              // Variabile che conterrà il danno dell'attacco pesante dell'arma

    // Flag tipo di arma
    public bool isSword = false;
    public bool isMace = false;
    public bool isAxe = false;

    // Attivazione audio di gioco
    public GameObject audioLightAttack;
    public GameObject audioHeavyAttack;

    // Booleano di controllo
    bool isLightAttacking;
    bool isHeavyAttacking;

    // Danni armi
    private int lightSwordDmg = 35;
    private int heavySwordDmg = 60;

    private int lightMaceDmg = 45;
    private int heavyMaceDmg = 95;

    private int lightAxeDmg = 40;
    private int heavyAxeDmg = 75;

    // Booleano gestione trucco danno massimo
    private bool infiniteDmgFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isSword)             // Danni spada
        {
            lightDamage = lightSwordDmg;
            heavyDamage = heavySwordDmg;
        }
        else if (isMace)         // Danni mazza
        {
            lightDamage = lightMaceDmg;
            heavyDamage = heavyMaceDmg;
        }
        else if (isAxe)          // Danni ascia
        {
            lightDamage = lightAxeDmg;
            heavyDamage = heavyAxeDmg;
        }
        //HPenemy = GameObject.FindWithTag("Enemy").GetComponent<Turtle>();
        animatorPlayer = GameObject.FindWithTag("PlayerModel").GetComponent<Animator>();  // Riferimento all'animator del player
    }

    private bool getState(string animationName) { return animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName(animationName); }

    // Update is called once per frame
    void Update()
    {
        // Qualunque sia il tipo di arma, controllo se l'animazione in corso è quella di un attacco leggero o pesante
        isLightAttacking = getState("LightSword") || getState("LightMace") || getState("LightAxe");
        isHeavyAttacking = getState("HeavySword") || getState("HeavyMace") || getState("HeavyAxe");

        if (isLightAttacking || isHeavyAttacking)
        {
            GetComponent<BoxCollider>().enabled = true;
            GetComponent<BoxCollider>().isTrigger = true;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
        }
        if (isLightAttacking)
        {
            if (infiniteDmgFlag) damageAmount = 9999;
            else damageAmount = lightDamage;

            audioLightAttack.SetActive(true);
        }
        else if (isHeavyAttacking)
        {
            if (infiniteDmgFlag) damageAmount = 9999;
            else damageAmount = heavyDamage;

            audioHeavyAttack.SetActive(true);
        }
        else
        {
            damageAmount = 0;
            audioLightAttack.SetActive(false);
            audioHeavyAttack.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collisione = other.gameObject;
        SmallEnemy nemico = collisione.GetComponent<SmallEnemy>();

        Boss boss = collisione.GetComponent<Boss>(); 

        // danno ai nemici piccoli
        if (nemico)
            nemico.TakeDamage(damageAmount);
        


        // danno al boss
        if (boss && isBoss)
            boss.TakeDamage(damageAmount);
    }

    public void InfiniteDmgCheat()
    {
        infiniteDmgFlag = !infiniteDmgFlag;
    }
}
