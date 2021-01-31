using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Poblador : MonoBehaviour
{
	public bool mostrarGizmos;
    public MeshGenerator generador;
    public Color[] colorCasos;
	public List<Casilla> bordes = new List<Casilla>();
	public List<Casilla> piso = new List<Casilla>();
	[Header("Jugabilidad")]
	public Transform jugador;

	[Header("Entorno")]
	public PosiblesParedes[] posiblesParedes;
	public PosiblesParedes casos0;
	public Transform padreEntorno;

	[Range(0f,1f)]
	public float posibleCaso0;
	[Range(0f, 1f)]
	public float posibleParedes;

	[Header("Enemigos")]
	public GameObject[] enemigos;
	[Range(0f, 1f)]
	public float posibleEnemigos;
	public Transform padreEnemigos;

	private IEnumerator Start()
    {
		yield return new WaitForSeconds(0.01f);
        if (generador == null)
        {
            generador = GetComponent<MeshGenerator>();
        }
		DetectarEspacios();
		yield return new WaitForSeconds(.05f);
		Rellenar();
		yield return new WaitForSeconds(.05f);
		Poblar();
		jugador.position = piso[Random.Range(0, piso.Count)].posicion + Vector3.up*1.2f;
		Portal.singleton.transform.position = piso[Random.Range(0, piso.Count)].posicion;
		Portal.singleton.transform.transform.Rotate(Vector3.up * Random.Range(0, 4) * 90);
	}

	public void DetectarEspacios()
    {
		if (generador.squareGrid != null)
		{
			for (int x = 0; x < generador.squareGrid.squares.GetLength(0); x++)
			{
				for (int y = 0; y < generador.squareGrid.squares.GetLength(1); y++)
				{
					Vector3 pos = generador.squareGrid.squares[x, y].centreTop.position;
					Vector3 dif = (generador.squareGrid.squares[x, y].centreTop.position - generador.squareGrid.squares[x, y].centreBottom.position) / 2;
					pos -= dif;
					Casilla c = new Casilla();
					c.posicion = pos;
					c.valor = generador.squareGrid.squares[x, y].configuration;
					if (c.valor == 0)
                    {
						piso.Add(c);
                    }
                    else if (c.valor < 15)
                    {
						bordes.Add(c);
                    }
				}
			}
		}
	}

	public void Rellenar()
	{
		// ------------------------------------------> Casos
		for (int i = 0; i < bordes.Count; i++)
        {
            if (posiblesParedes[bordes[i].valor].activo)
            {
				GameObject g = posiblesParedes[bordes[i].valor].GetPosible();
                if (g != null)
                {
                    if (Random.Range(0f,1f)<posibleParedes)
                    {
						Instantiate(g, bordes[i].posicion, Quaternion.identity, padreEntorno);
                    }
                }
            }
        }


		// ------------------------------------------> Pisos
        for (int i = 0; i < piso.Count; i++)
        {
			if (casos0.activo && Random.Range(0f, 1f) < posibleCaso0)
			{
				GameObject g = casos0.GetPosible();
				Instantiate(g, piso[i].posicion, Quaternion.Euler(0, Random.Range(0,4)*90 , 0), padreEntorno);
			}
		}
    }

	void Poblar()
    {
		for (int i = 0; i < piso.Count; i++)
		{
			if (Random.Range(0f, 1f) < (posibleEnemigos/10f))
			{
				GameObject g = enemigos[Random.Range(0,enemigos.Length)];
				Instantiate(g, piso[i].posicion, Quaternion.Euler(0, Random.Range(0, 4) * 90, 0), padreEnemigos);
			}
		}
	}

	void OnDrawGizmos()
	{
		if (mostrarGizmos && bordes != null)
		{
            for (int i = 0; i < bordes.Count; i++)
            {
				Gizmos.color = colorCasos[bordes[i].valor];
				Gizmos.DrawCube(bordes[i].posicion, Vector3.one * 0.3f);
            }
		}
	}
	[System.Serializable]
	public class Casilla
    {
		public Vector3 posicion;
		public int valor;
    }
}
[System.Serializable]
public class PosiblesParedes
{
	public GameObject[] posibles;
	public bool activo;
	public GameObject GetPosible()
    {
        if (posibles == null || posibles.Length==0)
        {
			return null;
        }
		return (posibles[Random.Range(0, posibles.Length)]);

    }
}