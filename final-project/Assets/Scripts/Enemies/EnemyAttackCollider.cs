using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttackCollider : MonoBehaviour
{
    public Animator enemyAnimator;

    HP_Player HPplayer;
    int dmg;                        // danno
    int turtleDmg = 25;
    int slimeDmg = 20;
    int scarabeoDmg = 35;
    int bossDmg = 40;
    //public bool isBoss = false;     // controlla se l'enemy è il boss

    bool isEnemyAttacking = false;  // controlla se l'enemy è in animazione di attacco

    public BoxCollider boxColliderAttack;   // utile per disattivare e riattivare il collider dell'attacco dell'enemy

    // controlla se l'animazione del nemico passata come parametro è attiva
    private bool getEnemyState(string animationName) { return enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName); }

    private void Awake()
    {
        HPplayer = GameObject.Find("Player").GetComponent<HP_Player>();

        // gestione danni
        if (SceneManager.GetActiveScene().name == "Boss Arena") dmg = bossDmg;          // se è nella mappa del boss, danno del boss
        else if (SceneManager.GetActiveScene().name == "Level_1") dmg = turtleDmg;      // se è nella mappa 1, danno del turtle
        else if (SceneManager.GetActiveScene().name == "Level_2") dmg = slimeDmg;       // se è nella mappa 2, danno dello slime
        else if (SceneManager.GetActiveScene().name == "Level_3") dmg = scarabeoDmg;    // se è nella mappa 3, danno dello scarabeo
    }

    // Update is called once per frame
    void Update()
    {
        isEnemyAttacking = getEnemyState("Stab Attack") || getEnemyState("Attack"); // controlla se l'enemy sta attaccando
        
        if (isEnemyAttacking) boxColliderAttack.enabled = true; // se sta attaccando attiva il box collider dell'enemy
        else boxColliderAttack.enabled = false;                 // altrimenti lo disattiva
    }                                                           // questo è utile per gestire i danni con OnTriggerEnter

    private void OnTriggerEnter(Collider other)
    {
        
        GameObject collisione = other.gameObject;                   // Ottieni l'oggetto con cui c'è stata una collisione 
        HP_Player player = collisione.GetComponent<HP_Player>();    // Cerca un componente HP_Player nell'oggetto con cui c'è stata la collisione

        // Verifica se è stato trovato un componente HP_Player e se l'enemy sta attaccando
        if (player && isEnemyAttacking)
        {          
            player.TakeDamage(dmg); // Causa danno al player richiamando il metodo TakeDamage del componente HP_Player
        }
    }

}
