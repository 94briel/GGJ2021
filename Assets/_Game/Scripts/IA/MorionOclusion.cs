using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorionOclusion : MonoBehaviour
{
    GameObject hijo;
    public float offcetInicial;

    void Start()
    {
        hijo = transform.parent.gameObject;
        transform.parent = transform.parent.parent ;
        if ((transform.position - Control.singleton.jugador.transform.position).sqrMagnitude > offcetInicial*offcetInicial)
        {
            Desactivar();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activar();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Desactivar();
        }
    }

    void Activar()
    {
        hijo.SetActive(true);
        //Destroy(gameObject);
    }

    void Desactivar()
    {
        if (hijo != null)
        {
            hijo.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
