using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Instanzia dei nemici in 3 punti specifici
public class SpawnTriggerLvl2 : MonoBehaviour
{
    public bool zona1 = false;  // Flag di controllo
    public bool zona2 = false;  // Flag di controllo
    public bool zona3 = false;  // Flag di controllo
    public GameObject enemy;    // Nemico da generare

    IEnumerator EnemyDrop()
    {
        // Genera l'imboscata nella prima zona, istanziando 3 nemici
        if (zona1)
        {
            Instantiate(enemy, new Vector3(87.4f, 14.8f, 159.3f), Quaternion.identity);
            Instantiate(enemy, new Vector3(74.3f, 14.8f, 139), Quaternion.identity);
            Instantiate(enemy, new Vector3(102.7f, 14.84f, 139), Quaternion.identity);
        }
        // Genera l'imboscata nella prima zona, istanziando 2 nemici
        else if (zona2)
        {
            Instantiate(enemy, new Vector3(119.3f, 14.84f, 174.65f), Quaternion.identity);
            Instantiate(enemy, new Vector3(100.82f, 14.84f, 174.65f), Quaternion.identity);
        }
        // Genera l'imboscata nella prima zona, istanziando 3 nemici
        else if (zona3)
        {
            Instantiate(enemy, new Vector3(126.08f, 14.84f, 164.15f), Quaternion.identity);
            Instantiate(enemy, new Vector3(126.08f, 14.84f, 137.35f), Quaternion.identity);
            Instantiate(enemy, new Vector3(143.47f, 14.84f, 150.5f), Quaternion.identity);
        }

        yield return new WaitForSeconds(0.1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(EnemyDrop());
            Destroy(gameObject);
        }
    }
}
