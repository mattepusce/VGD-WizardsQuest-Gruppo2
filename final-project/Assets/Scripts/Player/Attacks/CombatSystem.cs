using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CombatSystem : MonoBehaviour
{
    public Weapon currentWeapon;

    public GameObject attackUI;
    public Animator animator;
    public CharacterController controller;
    PlayerInputs playerInputs;

    // attack
    public float attackDuration;                    // durata dell'attacco
    public float attackCooldown = 1f;               // tempo di attesa per effettuare un nuovo attacco          
    private float attackStartTime = 0f;             // tempo di inizio dell'attacco
    public float heavyAttackHoldTime = 0.2f;        // Il tempo (in secondi) necessario per tenere premuto il tasto del mouse per attivare l'attacco pesante
    float lastReleasedMouseButtonTime = 1f;         // tempo dell'ultimo rilascio del tasto sinistro del mouse
    float lastReleasedMouseButtonTimeHeavy = 1f;    // tempo dell'ultimo rilascio del tasto sinistro del mouse per un heavy attack
    private bool canLightAttack = false;            // bool per attivare la possibilità di attacco leggero al player
    private bool canHeavyAttack = false;            // bool per attivare la possibilità di attacco pesante al player

    //shield
    float shieldCooldown = 1f;                      // tempo di attesa per effettuare una nuova parata
    private float lastRightButtonClickTime = 0f;    // tempo dell'ultimo click del taasto destro del mouse
    public bool canShield = true;                   // // bool per attivare la possibilità di parata al player

    // ----------------------------------------------------------------------------

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInputs = new PlayerInputs();
    }


    // ----------------------------------------------------------------------------



    private void HandleAttack(params string[] weapons)
    {
        foreach (string weapon in weapons)
        {
            if (animator.GetBool(weapon))       // se la spada è equipaggiata
            {
                if (Mouse.current.leftButton.wasPressedThisFrame) attackStartTime = Time.time;      // quando il tasto viene premuto, salva l'istante di inizio attacco

                if (Mouse.current.leftButton.wasReleasedThisFrame)  // quando il tasto viene rilasciato
                {
                    attackDuration = Time.time - attackStartTime;   // calcola il tempo in cui il tasto di attacco è rimasto premuto

                    canLightAttack = true;      // setta canLightAttack a true
                    canHeavyAttack = true;      // setta canHeavyAttack a true

                    if (Time.time - lastReleasedMouseButtonTime < currentWeapon.lightAttackCooldown)    // se dall'ultimo rilascio del tasto non è passato il tempo di cooldown minimo
                    {
                        canLightAttack = false;     // setta canLightAttack a false
                        canHeavyAttack = false;     // setta canHeavyAttack a false
                    }
                    else if (Time.time - lastReleasedMouseButtonTimeHeavy < currentWeapon.heavyAttackCooldown)  // se dall'ultimo rilascio del tasto per heavy attack non è passato il tempo di cooldown di heavy attack
                    {
                        canHeavyAttack = false;     // setta canHeavyAttack a false
                    }
                    
                    
                    if (canLightAttack && attackDuration < heavyAttackHoldTime)  // se il player può attaccare leggero e la durata è inferiore al tempo necessario per eseguire l'attacco pesante
                    {
                        Attack("Light" + weapon);     // esegui l'attacco leggero
                    }
                    else if (canHeavyAttack && attackDuration >= heavyAttackHoldTime) // se il player può attaccare pesante e la durata è superiore al tempo necessario per eseguire l'attacco pesante
                    {
                        Attack("Heavy" + weapon);                       // esegui l'attacco pesante
                        lastReleasedMouseButtonTimeHeavy = Time.time;   // salva il tempo dell'ultimo rilascio del tasto per heavy attack 
                    }

                    if (canLightAttack || canHeavyAttack) lastReleasedMouseButtonTime = Time.time;      // salva il tempo del rilascio del tasto
                                                                                                        // tiene conto del click solo se il player poteva attaccare
                }
            }
        }
    }

    void HandleShield()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)      // se viene premuto il tasto destro del mouse
        {
            canShield = true;   // canShield settato a true

            if (Time.time - lastRightButtonClickTime < shieldCooldown) canShield = false;    // se dall'ultimo click del tasto non è passato il tempo di cooldown, setta canShield a false
            if (canShield) animator.SetTrigger("Shield");                                   // se il player può proteggersi, attiva l'animazione shield

            lastRightButtonClickTime = Time.time;       // salva il tempo del click
        }
    }


    void HandleUI()
    {
        if (Time.time - lastReleasedMouseButtonTimeHeavy < currentWeapon.heavyAttackCooldown)
        {
            attackUI.SetActive(false);
        }
        else 
        {
            attackUI.SetActive(true);
        }
    }


    // ----------------------------------------------------------------------------

    void Update()
    {
        HandleAttack("Sword", "Axe", "Mace");
        HandleShield();
        HandleUI();
    }


    // ----------------------------------------------------------------------------

    // Attack setta il trigger per attivare l'animazione di attacco passata come parametro
    private void Attack(string animationName) { animator.SetTrigger(animationName); }

    private void OnEnable() { playerInputs.Player.Enable(); }       // abilitazione degli input
    private void OnDisable() { playerInputs.Player.Disable(); }     // disabilitazione degli input



}