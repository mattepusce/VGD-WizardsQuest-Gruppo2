using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;    // Bool di controllo
    public static bool canPressEsc = true;      // Bool gestito da DialogueManager.cs
    public GameObject pauseMenuUI;
    public CombatSystem combatSystem;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPressEsc)
        {
            if (GameIsPaused)
            {
                Resume();
                Cursor.visible = false;                      // Nascondo il cursore
                Cursor.lockState = CursorLockMode.Locked;    // Blocco il cursore
                combatSystem.enabled = true;                 // Attivo lo script CombatSystem del player
            } else
            {
                Pause();
                Cursor.visible = true;                       // Mostro il cursore
                Cursor.lockState = CursorLockMode.None;      // Sblocco il cursore
                combatSystem.enabled = false;                // Disattivo lo script CombatSystem del player
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);       // Nascondo il menu di pausa
        Time.timeScale = 1f;                // Riprendo lo scorrere del tempo
        GameIsPaused = false;               // Aggiorno bool di controllo
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);        // Mostro il menu di pausa
        Time.timeScale = 0f;                // Fermo lo scorrere del tempo
        GameIsPaused = true;                // Aggiorno bool di controllo
    }
}
