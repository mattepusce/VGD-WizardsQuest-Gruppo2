using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(PlayerController player)
    {       
        BinaryFormatter formatter = new BinaryFormatter();          // Crea un oggetto BinaryFormatter per serializzare i dati       
        string path = Application.persistentDataPath + "/game.txt"; // Definisci il percorso del file di salvataggio     
        FileStream stream = new FileStream(path, FileMode.Create);  // Crea un FileStream per creare o sovrascrivere il file di salvataggio       
        LevelData gameSaveData = new LevelData(player);             // Crea un oggetto LevelData che conterrà i dati da salvare
        formatter.Serialize(stream, gameSaveData);                  // Serializza l'oggetto LevelData nel FileStream
        stream.Close();                                             // Chiudi il FileStream
    }

    public static LevelData LoadData()
    {        
        string path = Application.persistentDataPath + "/game.txt";     // Definisci il percorso del file di salvataggio

        // Verifica se il file di salvataggio esiste
        if (File.Exists(path))
        {            
            BinaryFormatter formatter = new BinaryFormatter();              // Crea un oggetto BinaryFormatter per deserializzare i dati          
            FileStream stream = new FileStream(path, FileMode.Open);        // Crea un FileStream per aprire il file di salvataggio           
            LevelData data = formatter.Deserialize(stream) as LevelData;    // Deserializza i dati dal FileStream in un oggetto LevelData           
            stream.Close();                                                 // Chiudi il FileStream           
            return data;                                                    // Restituisci l'oggetto LevelData deserializzato
        }
        else
        {
            // Se il file non esiste, genera un errore e restituisci null
            Debug.LogError("File not found");
            return null;
        }
    }
}
