using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;              // Riferimento all'oggetto di gioco che contiene le frasi
    public GameObject dialogueUI;          // Riferimento all'oggetto di gioco che gestisce la UI del dialogo
    public GameObject portal;              // Riferimento all'oggetto di gioco che gestisce il portale
    public GameObject portalUI;            // Riferimento all'oggetto di gioco che gestisce la UI del portale
    public GameObject dialogueTrigger;     // Riferimento all'oggetto di gioco che gestisce il trigger del Dialogo

    public bool isHubMap = true;           // Booleano di controllo che abilita l'attivazione del portale in Hub Map

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();  // Inizializzo la variabile
    }

    public void StartDialogue (Dialogue dialogue)
    {
        PauseMenu.canPressEsc = false;             // Il gioco non può essere messo in pausa
        Cursor.visible = true;                     // Mostro il cursore del mouse
        Cursor.lockState = CursorLockMode.None;    // Sblocco il cursore del mouse
        dialogueUI.SetActive(true);                // Attivo l'oggetto di gioco che gestisce la UI del dialogo
        sentences.Clear();                         // Se la coda conteneva già dialoghi, la svuoto
        Time.timeScale = 0f;                       // Fermo il tempo di gioco

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);   // Inserisco le frasi nella variabile sentences
        }

        DisplayNextSentence();    // Stampo la prima frase
    }

    public void DisplayNextSentence()
    {
        // Non ci sono più frasi, il dialogo è finito
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();  // Estraggo la prossima linea di dialogo
        dialogueText.text = sentence;           // Mostro la linea di dialogo appena estratta
    }

    void EndDialogue()
    {
        PauseMenu.canPressEsc = true;                      // Il gioco può essere messo in pausa
        Cursor.visible = false;                            // Nascondo il cursore del mouse
        Cursor.lockState = CursorLockMode.Locked;          // Blocco il cursore del mouse
        dialogueTrigger.SetActive(false);                  // Disattivo il trigger del dialogo
        dialogueUI.SetActive(false);                       // Disattivo la UI del dialogo
        Time.timeScale = 1f;                               // Faccio riprendere il tempo di gioco
                                                           
        // Blocco di codice da eseguire solo nella mappa Hub Map
        if (isHubMap)
        {
            portal.SetActive(true);                            // Attivo il portale
            portalUI.SetActive(true);                          // Attivo la UI che notifica l'attivazione del portale al giocatore
        }
    }
}
