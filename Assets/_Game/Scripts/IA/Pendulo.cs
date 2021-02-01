using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulo : MonoBehaviour
{
    
    void FixedUpdate()
    {
        if (EnemigoGrande.singleton != null)
        {
            transform.LookAt(EnemigoGrande.singleton.transform);
        }
        else
        {
            transform.LookAt(Portal.singleton.transform);
        }
    }
}
