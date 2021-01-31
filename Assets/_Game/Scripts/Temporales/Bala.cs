using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bala : MonoBehaviour
{
    public GameObject particulasFinal;
    public float tiempoVida = 5f;
    public float velocidad;
    public float daño;
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * velocidad;
        Destroy(gameObject, tiempoVida);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Vida  
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (particulasFinal != null)
        {
            Destroy(Instantiate(particulasFinal, transform.position, Quaternion.identity) as GameObject, tiempoVida);
        }
    }
}
