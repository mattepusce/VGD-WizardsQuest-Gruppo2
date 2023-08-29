using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeLVL : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        // Se avviene una collisione tra il trigger e l'oggetto Player, viene caricato il livello successivo
        if(other.tag == "Player")
            SceneManager.LoadScene(sceneName);
    }
}
