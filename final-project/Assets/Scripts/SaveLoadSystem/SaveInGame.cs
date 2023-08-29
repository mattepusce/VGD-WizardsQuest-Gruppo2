using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveInGame : MonoBehaviour
{
    public PlayerController player;

    private void Awake()
    {       
        string path = Application.persistentDataPath + "/game.txt"; // Ottieni il percorso completo del file di salvataggio
        Debug.Log(path);

        if (SceneManager.GetActiveScene().name != "Main Menu")  // Se la scena attuale non è il menu principale, salva il giocatore
            SavePlayer();                                       // Evita di salvare quando si apre il menu principale
    }

    public void SavePlayer()
    {
        SaveSystem.SaveData(player); // Chiama il metodo di salvataggio dal SaveSystem
    }

    public void LoadPlayer()
    {       
        string path = Application.persistentDataPath + "/game.txt"; // Ottieni il percorso completo del file di salvataggio

        // Controlla se il file di salvataggio esiste
        if (File.Exists(path))
        {            
            LevelData data = SaveSystem.LoadData(); // Carica i dati del giocatore utilizzando il metodo di caricamento dal SaveSystem            
            SceneManager.LoadScene(data.levelName); // Carica la scena corretta e imposta il nome del livello
            Time.timeScale = 1f;                    // riporta lo scorrimento del tempo di gioco a 1 
        }
    }
}
