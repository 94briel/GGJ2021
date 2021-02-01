using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaPowerUp : MonoBehaviour
{
    public float cuantaVida = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            Control.singleton.jugador.GetComponent<Vida>().SumarVida(cuantaVida);
        }
    }
}
