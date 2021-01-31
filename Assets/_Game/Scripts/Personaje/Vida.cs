using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vida : MonoBehaviour
{
    public float vidaInicial;
    public float vidaActal;
    public UnityEvent eventoMorir;

    public bool vivo = true;
    private void Start()
    {
        vidaActal = vidaInicial;
    }
    public void CausarDaño(float cuanto)
    {
        if (!vivo) return;
        vidaActal -= cuanto;
        if (vidaActal <= 0)
        {
            vivo = false;
            eventoMorir.Invoke();
        }
    }

    public float GetSangre()
    {
        return vidaActal / vidaInicial;
    }
}
