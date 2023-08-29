using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmallEnemy : MonoBehaviour
{
    private int HP;                     // hp del nemico
    private int maxHP;                  // vita massima che varia a seconda dei nemici
    private int turtleMaxHP = 90;       // vita massima di turtle
    private int slimeMaxHP = 100;       // vita massima di slime
    private int scarabeoMaxHP = 170;    // vita massima di scarabeo
    private string levelName;           // nome del livello

    public Animator enemyAnimator;     // animator del nemico
    private Animator playerAnimator;    // animator del player

    void Awake()
    {
        levelName = SceneManager.GetActiveScene().name; // nome del livello corrente

        // vita massima del nemico settata in base al livello, e quindi in base al tipo di nemico
        if(levelName == "Level_1") maxHP = turtleMaxHP;
        else if (levelName == "Level_2") maxHP = slimeMaxHP;
        else if (levelName == "Level_3") maxHP = scarabeoMaxHP;
        
        HP = maxHP;     // la vita iniziale del nemico è pari ai suoi hp massimi
        enemyAnimator = GetComponent<Animator>();   // assegnamento dell'animator del nemico

        // il seguente codice serve per associare l'animator del player allo script di ogni enemy
        GameObject playerGameObject = GameObject.Find("Player"); // L'oggetto vuoto che rappresenta il player
        if (playerGameObject != null)
        {
            // Trova il figlio dell'oggetto vuoto (dove è associato l'Animator)
            Transform playerAnimatorTransform = playerGameObject.transform.Find("RPGHeroPolyart");
            if (playerAnimatorTransform != null)
            {
                // Ottieni il componente Animator dal figlio e assegnalo
                playerAnimator = playerAnimatorTransform.GetComponent<Animator>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // controllo se il player è in animazione di attacco
        bool isPlayerAttacking = getState("LightSword") || getState("HeavySword") || getState("LightMace") || getState("HeavyMace") || getState("LightAxe") || getState("HeavyAxe");
        
        // ri attivo il box collider dell'enemy quando l'animazione di attacco del player finisce
        if (!isPlayerAttacking && HP > 0)
            gameObject.GetComponent<BoxCollider>().enabled = true;


    }

    // funzione di danno al nemico
    public void TakeDamage(int damage)
    {
        if (HP > 0) // se il nemico ha ancora vita
        {
            HP -= damage;   // applica il danno
            Debug.Log("il nemico ha preso danno: " + damage + " all'istante " + Time.time);
            gameObject.GetComponent<BoxCollider>().enabled = false; // disattivo il box collider dell'enemy quando prende danno
            enemyAnimator.SetTrigger("damage");  // attiva l'animazione di danno

            if (HP <= 0)    // se il nemico non ha più vita
            {
                enemyAnimator.SetTrigger("die"); // attiva l'animazione di morte                    
                if (KillCounter.killcount > 0)  // se il kill counter non vale già zero
                    KillCounter.killcount--;    // diminuisci di uno il kill counter
                gameObject.GetComponent<BoxCollider>().enabled = false; // disattiva il box colider del nemico
                Object.Destroy(gameObject, 3f); // distruzione del game object del nemico
            }
        }
    }

    // funzione utile per avere lo stato dell'animazione passata come parametro
    private bool getState(string animationName) { return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName); }
    
}
