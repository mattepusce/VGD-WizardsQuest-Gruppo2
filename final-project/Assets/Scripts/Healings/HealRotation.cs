using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0f, 0.5f, 0f);  // Rotazione cura, puramente estetico
    }
}
