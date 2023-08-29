using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Ruoto la barra della vita in maniera tale che sia sempre rivolta verso la visuale del giocatore
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
