using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    float speed = 30f;   // Velocità palla di fuoco
    
    // Start is called before the first frame update
    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);   // Trasla in linea retta l'oggetto
        Destroy(gameObject, 5);     
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collisione = other.gameObject;    // Riferimento tramite script...
        HP_Player player = collisione.GetComponent<HP_Player>(); // dello script associato al player
        
        // Viene controllato che il player collida
        if(player)
        {
            player.TakeDamage(40);   // Infliggo danno al giocatore
            Destroy(gameObject);     // Distruggo la palla di fuoco
        }
    }
}
