using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vida : MonoBehaviour
{
    public float vidaInicial;
    public float vidaActal;
    public UnityEvent eventoMorir;


    private void Start()
    {
        vidaActal = vidaInicial;
    }
    public void CausarDaño(float cuanto)
    {
        vidaActal -= cuanto;
        if (vidaActal <= 0)
        {
            eventoMorir.Invoke();
        }
    }

    public float GetSangre()
    {
        return vidaActal / vidaInicial;
    }
}
