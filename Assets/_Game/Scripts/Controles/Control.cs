using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public static Control singleton;
    public GameObject jugador;
    public EstadoTalisman estadoTalisman;

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
        
    }
}

public enum EstadoTalisman
{
    bien = 0,
    mal = 1
}