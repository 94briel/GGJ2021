using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Poblador : MonoBehaviour
{
	public bool mostrarGizmos;
	public NavMeshSurface[] mallaNavegacion;
    public MeshGenerator generador;
    public Color[] colorCasos;
	public List<Casilla> bordes = new List<Casilla>();
	public List<Casilla> piso = new List<Casilla>();

	public PosiblesParedes[] posiblesParedes;


    private IEnumerator Start()
    {
		yield return new WaitForSeconds(0.1f);
        if (generador == null)
        {
            generador = GetComponent<MeshGenerator>();
        }
		DetectarEspacios();
		yield return new WaitForSeconds(1f);
		Rellenar();

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
        for (int i = 0; i < bordes.Count; i++)
        {
            if (posiblesParedes[bordes[i].valor].activo)
            {
				GameObject g = posiblesParedes[bordes[i].valor].GetPosible();
                if (g != null)
                {
					Instantiate(g, bordes[i].posicion, Quaternion.identity);
                }
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