using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;            // Gestione Audio
    public CinemachineFreeLook cinemachine;  // Gestione Camera

    // Gestisce il volume di gioco
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    // Gestisce la sensibilità orizzontale del Mouse
    public void SetSensibilityX(float sensX)
    {
        cinemachine.m_XAxis.m_MaxSpeed = sensX;  // Riferimento a CineMachine
        //Debug.Log("Valore X = " + sensX);
    }

    // Gestisce la sensibilità verticale del Mouse
    public void SetSensibilityY(float sensY)
    {
        cinemachine.m_YAxis.m_MaxSpeed = sensY;  // Riferimento a CineMachine
        //Debug.Log("Valore Y = " + sensY);
    }
}
