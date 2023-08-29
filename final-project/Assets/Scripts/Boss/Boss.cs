using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Gestisce il comportamento del boss in base a determinate condizioni, oltre a gestire la vita
public class Boss : MonoBehaviour
{
    public GameObject VictoryUI;
    Animator animator;

    Transform player;

    private float bulletTime = 0.5f;

    public GameObject enemyBullet;
    public Transform spawnPoint;

    public HPBarScript health;        //faccio un riferimento allo script HPBarScript

    [SerializeField]
    public static int maxHP = 500;     // Vita massima Boss
    [SerializeField]
    public static int currentHP;       // Contiene la vita attuale del boss

    public PlayerController playerController;       // Riferimento allo script playerController
    public CombatSystem combatSystem;               // Riferimento allo script combatSystem
    private Animator playerAnimator;                // Riferimento all'animator del Player
    public GameObject gameMusic;                    // Riferimento all'oggetto che gestisce la musica d'ambiente

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        animator.SetBool("Fase2", false);

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

    public void TakeDamage(int damage)    //il boss prende danno
    {
        if (currentHP > 0)
        {
            currentHP -= damage;              //diminuisce il valore
            health.SetHealth(currentHP);           //setta il valore sulla barra degli hp
            gameObject.GetComponent<BoxCollider>().enabled = false;   //non prende danno ogni frame 

        }
        if (currentHP <= 0)
        {
            animator.SetTrigger("die");         //Attiva l'animazione di morte
            gameObject.GetComponent<BoxCollider>().enabled = false;
            VictoryUI.SetActive(true);          //UI di vittoria
            playerController.enabled = false;
            combatSystem.enabled = false;
            gameMusic.SetActive(false);
        }
    }

    void Shoot()     //casta le palle di fuoco
    {
        bulletTime -= Time.deltaTime;      //cooldown a ogni cast
        if (bulletTime > 0)
            return;

        bulletTime = 0.5f;
        animator.transform.LookAt(player);
        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;  //istanzio il prefab Fireball
    }

    // Update is called once per frame
    void Update()
    {
        bool isPlayerAttacking = getState("LightSword") || getState("HeavySword") || getState("LightMace") || getState("HeavyMace") || getState("LightAxe") || getState("HeavyAxe");

        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (!isPlayerAttacking && currentHP > 0)    //Se il giocatore è in animazione di attacco il nemico può prendere danno
            gameObject.GetComponent<BoxCollider>().enabled = true;


        if (currentHP > 0)
        {
            if (animator.GetBool("isCasting"))
            {
                if (currentHP <= 250 && distance >= 10f)
                    Shoot();
            }

            if (currentHP <= 250)
                animator.SetBool("Fase2", true);
        }
        else
        {
            animator.SetBool("Fase2", false);
        }
    }

    private bool getState(string animationName) { return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName); }
}
