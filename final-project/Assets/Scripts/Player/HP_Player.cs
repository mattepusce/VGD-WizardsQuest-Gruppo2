using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HP_Player : MonoBehaviour
{
    // Riferimenti a oggetti di gioco
    public GameObject DeathUI;
    public static int deathCounter = 3;
    private bool isShielding = false;
    public Animator animator;

    public static int HP;
    public static int maxHP = 300;
    bool invulnerability = false;

    public GameObject DeathAudio;

    public float spatialBlend;

    public Text LifeText;

    public SaveInGame player;
    public AudioClip deathClip;


    private void Start()
    {
        HP = maxHP;
    }

    private Animator playerAnimator; // Variabile privata per l'Animator del player

    // Metodo pubblico per assegnare il valore a playerAnimator
    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    private bool getState(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    private void Update()
    {
        isShielding = getState("Shield");

        ShowLife();
    }

    public void TakeDamage(int damage)
    {
        if (!isShielding && !invulnerability)  //il player è stato colpito recentemente?
        {
            if (HP > 0)
            {
                HP -= damage;
                if (HP <= 0)
                {
                    Die();
                }
            }
        }

    }

    // Funzione onTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lava")
        {
            HP = 0;
            Die();
        }
        if (other.tag == "Cactus")
        {
            HP -= 20;
        }
    }

    public void Energiga(int Cura)
    {
        HP += Cura;
        if (HP > maxHP)
            HP = maxHP;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5);
        if (deathCounter <= 0)
        {
            SceneManager.LoadScene("Hub Map");
            deathCounter = 3;
        }
        else player.LoadPlayer();
    }

    public void Die()
    {
        DeathUI.SetActive(true);
        DeathAudio.SetActive(true);
        animator.SetTrigger("die");
        StartCoroutine(Respawn());
        deathCounter--;
    }

    void ShowLife()
    {
        LifeText.text = deathCounter.ToString();
    }

    public void InfiniteLifeCheat()
    {
        invulnerability = !invulnerability;
    }
}
