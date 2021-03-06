﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class Control : MonoBehaviour
{
    public static Control singleton;
    public GameObject jugador;
    public EstadoTalisman estadoTalisman;
    public PostProcessVolume ppVolume;
    public PostProcessProfile perfilBien;
    public PostProcessProfile perfilMal;

    public GameObject mallaBien;
    public GameObject mallaMal;

    public UnityEvent eventoCambiaModo;

    private void Awake()
    {
        singleton = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CambiarModo();
        }
    }

    void CambiarModo()
    {
        if (estadoTalisman == EstadoTalisman.bien)
        {
            estadoTalisman = EstadoTalisman.mal;
        }
        else
        {
            estadoTalisman = EstadoTalisman.bien;
        }

        eventoCambiaModo.Invoke();

        mallaBien.SetActive(estadoTalisman == EstadoTalisman.bien);
        mallaMal.SetActive(estadoTalisman == EstadoTalisman.mal);

        switch (estadoTalisman)
        {
            case EstadoTalisman.bien:
                ppVolume.profile = perfilBien;
                break;
            case EstadoTalisman.mal:
                ppVolume.profile = perfilMal;
                break;
            default:
                break;
        }
    }
}

public enum EstadoTalisman
{
    bien = 0,
    mal = 1
}