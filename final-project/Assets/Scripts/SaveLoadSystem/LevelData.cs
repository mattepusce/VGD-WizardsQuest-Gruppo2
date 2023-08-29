using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Questa classe rappresenta le informazioni da salvare per un determinato livello di gioco
[System.Serializable]
public class LevelData
{
    public string levelName;    // Il nome del livello da salvare

    // Costruttore della classe LevelData
    public LevelData(PlayerController player)
    {        
        levelName = player.levelName;   // Assegna il nome del livello dal player alla variabile levelName
    }
}