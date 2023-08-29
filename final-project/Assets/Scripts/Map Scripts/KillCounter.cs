using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Gestisco il numero di uccisioni che avvengono nel livello
public class KillCounter : MonoBehaviour
{
    [SerializeField]
    public static int killcount;
    public GameObject portal;
    public GameObject portalUI;
    public bool lvl3 = false;

    public Text killcounterText;
    // Start is called before the first frame update
    void Start()
    {
        if (!lvl3)
        {
            killcount = 12;
            //killcount = 1;  //Testing
        }
        else
        {
            killcount = 5;
            //killcount = 1;  //Testing
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowKills();
        if (killcount == 0)
        {
            portal.SetActive(true);
            portalUI.SetActive(true);
        }
    }

    public void ShowKills()
    {
        killcounterText.text = killcount.ToString();
    }
}
