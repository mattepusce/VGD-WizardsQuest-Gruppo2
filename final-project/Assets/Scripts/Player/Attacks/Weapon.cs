using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float lightAttackCooldown;   // cooldown attacchi leggeri
    public float heavyAttackCooldown;   // cooldown attacchi pesanti
    public bool isWeaponEquipped;       // booleano per verificare se un'arma è equipaggiata
    public Animator animator;           // animator del player

    // booleani per controllare quale arma è equipaggiata
    public bool sword; 
    public bool mace;
    public bool axe;

    
    private void Update()
    {
        // aggiornamento dello stato del player dall'animator
        isWeaponEquipped = animator.GetBool("isWeaponEquipped");       
        sword = animator.GetBool("Sword");
        mace = animator.GetBool("Mace");
        axe = animator.GetBool("Axe");
    }
}