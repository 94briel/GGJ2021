using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Vida : MonoBehaviour
{
    public float vidaInicial;
    public float vidaActal;
    public UnityEvent eventoMorir;


    public Slider slider;

    public bool vivo = true;
    private void Start()
    {
        vidaActal = vidaInicial;
        if (slider != null)
        {
            slider.value = 1;
        }
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
        if (slider != null)
        {
            slider.value = GetSangre();
        }
    }

    public float GetSangre()
    {
        return vidaActal / vidaInicial;
    }
}
