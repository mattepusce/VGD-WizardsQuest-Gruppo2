using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    public GameObject TriggerObject;

    public static int killcount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (killcount == 12)
            Instantiate(TriggerObject, new Vector3(30.7f, 0.46f, 54), Quaternion.identity);
    }
}
