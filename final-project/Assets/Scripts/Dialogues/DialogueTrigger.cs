using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    // Non appena entro in collisione con il trigger del dialogo, lo avvio
    public void OnTriggerEnter (Collider other)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);  // Trovo DialogueManager e chiamo la funzione StartDialogue()
    }
}
