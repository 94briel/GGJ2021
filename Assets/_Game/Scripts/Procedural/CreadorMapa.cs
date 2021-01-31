using System.Collections;
using System;
using UnityEngine;


public class CreadorMapa : MonoBehaviour
{
    public int ancho;
    public int largo;

    public string semilla;
    public bool usarSemilla;

    [Range(0,100)]
    public int randomLlenado;
    int[,] mapa;

    private void Start()
    {
        GenerarMapa();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            GenerarMapa();
        }
        if ( Input.GetMouseButtonDown(1))
        {
            Suavizar();
        }

    }

    void GenerarMapa()
    {
        mapa = new int[ancho, largo];
        LlenarMapaRandom();
        for (int i = 0; i < 5; i++)
        {
            Suavizar();
        }


        int tamañoBorde = 1;
        int[,] bordeMapa = new int[ancho + tamañoBorde * 2, largo + tamañoBorde * 2];

        for (int i = 0; i < bordeMapa.GetLength(0); i++)
        {
            for (int j = 0; j < bordeMapa.GetLength(1); j++)
            {
                if (i >= tamañoBorde && i < ancho + tamañoBorde && j >= tamañoBorde && j < largo + tamañoBorde)
                {
                    bordeMapa[i, j] = mapa[i - tamañoBorde, j - tamañoBorde];
                }
                else
                {
                    bordeMapa[i, j] = 1;
                }
            }
        }

        GeneradorMalla mallaGenerada = GetComponent<GeneradorMalla>();
        mallaGenerada.GenerarMalla(bordeMapa, 1);
    }

    void LlenarMapaRandom()
    {
        if (usarSemilla)
        {
            semilla = Time.time.ToString();
        }

        System.Random rnd = new System.Random(semilla.GetHashCode());
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                if (i==0 || j==0 || i==ancho-1 || j==largo-1)
                {
                    mapa[i, j] = 1;
                }
                else
                {
                    mapa[i, j] = (rnd.Next(0, 100) < randomLlenado) ? 1 : 0;
                }
            }
        }
    }

    void Suavizar()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                int paredes = GetCuentaParedes(i, j);
                if (paredes > 4)
                {
                    mapa[i, j] = 1;
                }
                else if(paredes < 4)
                {
                    mapa[i, j] = 0;
                }
            }
        }
    }

    int GetCuentaParedes(int x, int y)
    {
        int cuenta = 0;
        for (int i = x-1; i <= x+1; i++)
        {
            for (int j = y-1; j <= y+1; j++)
            {
                if (i != x || j!=y)
                {
                    if (j>=0 && i>=0 && i<ancho && j<largo)
                    {
                        cuenta += mapa[i, j];
                    }
                    else
                    {
                        cuenta++;
                    }
                }
            }
        }
        return cuenta;
    }

    private void OnDrawGizmos()
    {
        /*
        if (mapa != null)
        {
            for (int i = 0; i < ancho; i++)
            {
                for (int j = 0; j < largo; j++)
                {
                    Gizmos.color = (mapa[i, j] == 1) ? Color.black: Color.white;
                    Vector3 pos = new Vector3(-ancho / 2 + i + .5f, 0, -largo / 2 + j + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
        */
    }
}
