using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainFunctions : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();  // Esci dal gioco
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);  // Carica il livello passato come parametro della funzione
    }

}
