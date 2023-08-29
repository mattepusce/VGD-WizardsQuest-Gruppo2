using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Healpoint cura il player
public class HealPoint : MonoBehaviour
{
    public GameObject HealingAudio;  // Riferimento al gameObject associato all'audio di cura
    
    private void OnTriggerEnter(Collider other)
    {
        GameObject collisione = other.gameObject;     
        HP_Player player = collisione.GetComponent<HP_Player>();   //Reference via script dell'oggetto player con lo script HP_Player
        
        // Il seguente blocco di codice si esegue se player esiste e se il giocatore ha meno della vita massima
        if (player && HP_Player.HP < HP_Player.maxHP)
        {
            if (other.gameObject.tag == "Player") // Se il tag corrisponde a Player, eseguo il codice
            {
                player.Energiga(100);          // Curo 100hp al giocatore
                HealingAudio.SetActive(true);  // Attivo audio di curo
                Destroy(gameObject);           // Distruggo il gameObject delle cure per evitare che possa essere riutilizzato
            }
        }
    }
}
