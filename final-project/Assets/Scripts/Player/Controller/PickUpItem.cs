using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpItem : MonoBehaviour
{
    public Animator animator;
    public Transform pickUpPoint;           // punto di attacco dell'arma al player
    public Transform player;
    public Vector3 pickUpRotation;          // rotazione dell'item quando viene preso
    private Vector3 standardRotation;       // rotazione iniziale del weapon (non preso)
    public GameObject pickUpSound;          // oggetto che lancia l'audio di pickUp dell'arma
    private bool isEquipped = false;

    private float pickUpDistance;           // distanza massima per pickup

    private bool itemIsPicked;              // controlla se l'item è in mano

    public Rigidbody rb;

    public GameObject pickUpUI;
    public GameObject[] otherWeapons;

    public bool bossFight = false;


    // ----------------------------------------------------------------------------

    void Start()
    {
        player = GameObject.Find("Player").transform;               // transform del player
        pickUpPoint = GameObject.Find("PickUpPoint").transform;     // transform del punto di attacco dell'item
        standardRotation = this.transform.localEulerAngles;         // rotazione utile per il drop dell'item, in modo che torni alla posizione precedente al pick up
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Debug.Log("F");

        pickUpDistance = Vector3.Distance(player.position, transform.position);     // calcolo della distanza tra il player e l'item

        if (pickUpDistance <= 2 && !isEquipped)     // se la distanza è minore di 2
        {
            pickUpUI.SetActive(true);  // Attivo UI
            //Debug.Log("PickUp Area check");
            if (Input.GetKeyDown(KeyCode.F) && itemIsPicked == false && pickUpPoint.childCount < 1)  // se si preme F e non si ha nessun item in mano
            {
                //Debug.Log("Key was pressed");
                OnPickUpItem("Sword", "Axe", "Mace");
                pickUpSound.SetActive(true);             // Suono di pickup dell'arma
                isEquipped = true;                       // Aggiornamento flag
                
                // Gestione altre armi nella mappa del boss
                if (bossFight)
                {
                    foreach (GameObject otherWeapon in otherWeapons)
                    {
                        otherWeapon.GetComponent<PickUpItem>().enabled = false; // Per ogni altra arma disattivo lo script PickUpItem
                        otherWeapon.GetComponent<WeaponDmg>().enabled = false; // Per ogni altra arma disattivo lo script WeaponDmg
                    }
                }
            }
        }
        else
        {
            pickUpUI.SetActive(false);  // Disattivo UI
        }
    }

    void OnPickUpItem(params string[] weaponNames)
    {
        foreach (string weaponName in weaponNames)
        {
            if (weaponName == this.gameObject.name)     // verifica quale arma viene equipaggiata
            {
                GetComponent<Rigidbody>().useGravity = false;                       // disattiva la gravità
                GetComponent<BoxCollider>().enabled = false;                        // disattiva il box collider
                this.transform.position = pickUpPoint.position;                     // posiziona l'item in mano al player
                this.transform.parent = GameObject.Find("PickUpPoint").transform;   // posiziona l'item come figlio di PickUpPoint per attaccarlo al player
                this.transform.localEulerAngles = pickUpRotation;                   // modifica della rotazione dell'item
                itemIsPicked = true;

                animator.SetBool(weaponName, true);             // utile per gestire le animazioni degli attacchi con armi
                animator.SetBool("isWeaponEquipped", true);     // utile per gestire la modalità di corsa con e senza arma
            }
        }
    }


    // ----------------------------------------------------------------------------



}
