using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [TextArea(3, 10)]            // Specifica che mi semplifica scrivere i dialoghi su unity (dimensione text area)
    public string[] sentences;   // Contiene le varie frasi del dialogo
}
