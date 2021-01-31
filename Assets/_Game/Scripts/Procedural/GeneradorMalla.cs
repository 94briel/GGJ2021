using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneradorMalla : MonoBehaviour
{
    public CuadriculaCuadros cuadriculaCuadros;
    public MeshFilter paredes;
    List<Vector3> vertices;
    List<int> triangulos;

    Dictionary<int, List<Triangulo>> diccionarioTriangulos = new Dictionary<int, List<Triangulo>>();
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkVertices = new HashSet<int>();
    public void GenerarMalla(int[,] mapa, float tamañoCuadro)
    {
        diccionarioTriangulos.Clear();
        outlines.Clear();
        checkVertices.Clear();

        vertices = new List<Vector3>();
        triangulos = new List<int>();
        cuadriculaCuadros = new CuadriculaCuadros(mapa, tamañoCuadro);

        for (int i = 0; i < cuadriculaCuadros.cuadros.GetLength(0); i++)
        {
            for (int j = 0; j < cuadriculaCuadros.cuadros.GetLength(1); j++)
            {
                TriangularCuadros(cuadriculaCuadros.cuadros[i, j]);
            }
        }
        Mesh malla = new Mesh();
        GetComponent<MeshFilter>().mesh = malla;

        malla.vertices = vertices.ToArray();
        malla.triangles = triangulos.ToArray();
        malla.RecalculateNormals();

        CrearMallaPared();
    }

    void CrearMallaPared()
    {
        CalcularBordesMalla();

        List<Vector3> verticesPared = new List<Vector3>();
        List<int> triangulosPared = new List<int>();
        Mesh mallaPared = new Mesh();
        float alturaPared = 5;

        foreach (List<int> outline in outlines)
        {
            for (int i = 0; i < outline.Count-1; i++)
            {
                int startIndex = verticesPared.Count;
                verticesPared.Add(vertices[outline[i]]); // izq
                verticesPared.Add(vertices[outline[i+1]]); // der
                verticesPared.Add(vertices[outline[i]] + Vector3.down * alturaPared); // Inf_izq
                verticesPared.Add(vertices[outline[i+1]] + Vector3.down * alturaPared); // inf_der

                triangulosPared.Add(startIndex + 0);
                triangulosPared.Add(startIndex + 2);
                triangulosPared.Add(startIndex + 3);

                triangulosPared.Add(startIndex + 3);
                triangulosPared.Add(startIndex + 1);
                triangulosPared.Add(startIndex + 0);

            }
        }

        mallaPared.vertices = verticesPared.ToArray();
        mallaPared.triangles = triangulosPared.ToArray();
        mallaPared.RecalculateNormals(); 
        paredes.mesh = mallaPared;

    }

    void TriangularCuadros(Cuadro cuadro)
    {
        switch (cuadro.configuracion)
        {
            case 0:
                break;
            // **************** Un solo vértice
            case 1:
                MallaDesdePuntos(cuadro.centreLeft, cuadro.centreBottom, cuadro.bottomLeft);
                break;
            case 2:
                MallaDesdePuntos(cuadro.bottomRight, cuadro.centreBottom, cuadro.centreRight);
                break;
            case 4:
                MallaDesdePuntos(cuadro.topRight, cuadro.centreRight, cuadro.centreTop);
                break;
            case 8:
                MallaDesdePuntos(cuadro.topLeft, cuadro.centreTop, cuadro.centreLeft);
                break;
            // **************** 2 vértices
            case 3:
                MallaDesdePuntos(cuadro.centreRight, cuadro.bottomRight, cuadro.bottomLeft, cuadro.centreLeft);
                break;
            case 6:
                MallaDesdePuntos(cuadro.centreTop, cuadro.topRight, cuadro.bottomRight, cuadro.centreBottom);
                break;
            case 9:
                MallaDesdePuntos(cuadro.topLeft, cuadro.centreTop, cuadro.centreBottom, cuadro.bottomLeft);
                break;
            case 12:
                MallaDesdePuntos(cuadro.topLeft, cuadro.topRight, cuadro.centreRight, cuadro.centreLeft);
                break;
            case 5:
                MallaDesdePuntos(cuadro.centreTop, cuadro.topRight, cuadro.centreRight, cuadro.centreBottom, cuadro.bottomLeft, cuadro.centreLeft);
                break;
            case 10:
                MallaDesdePuntos(cuadro.topLeft, cuadro.centreTop, cuadro.centreRight, cuadro.bottomRight, cuadro.centreBottom, cuadro.centreLeft);
                break;
            default:
                break;
            // **************** 3 vértices
            case 7:
                MallaDesdePuntos(cuadro.centreTop, cuadro.topRight, cuadro.bottomRight, cuadro.bottomLeft, cuadro.centreLeft);
                break;
            case 11:
                MallaDesdePuntos(cuadro.topLeft, cuadro.centreTop, cuadro.centreRight, cuadro.bottomRight, cuadro.bottomLeft);
                break;
            case 13:
                MallaDesdePuntos(cuadro.topLeft, cuadro.topRight, cuadro.centreRight, cuadro.centreBottom, cuadro.bottomLeft);
                break;
            case 14:
                MallaDesdePuntos(cuadro.topLeft, cuadro.topRight, cuadro.bottomRight, cuadro.centreBottom, cuadro.centreLeft);
                break;
            // **************** 4 vértices
            case 15:
                MallaDesdePuntos(cuadro.topLeft, cuadro.topRight, cuadro.bottomRight, cuadro.bottomLeft);
                checkVertices.Add(cuadro.topLeft.indice);
                checkVertices.Add(cuadro.topRight.indice);
                checkVertices.Add(cuadro.bottomRight.indice);
                checkVertices.Add(cuadro.bottomLeft.indice);
                break;


        }
    }

    void MallaDesdePuntos(params Nodo[] puntos)
    {
        AsignarVertices(puntos);
        if (puntos.Length >= 3)
        {
            CrearTriangulo(puntos[0], puntos[1], puntos[2]);
        }
        if (puntos.Length>=4)
        {
            CrearTriangulo(puntos[0], puntos[2], puntos[3]);
        }
        if (puntos.Length >= 5)
        {
            CrearTriangulo(puntos[0], puntos[3], puntos[4]);
        }
        if (puntos.Length >= 6)
        {
            CrearTriangulo(puntos[0], puntos[4], puntos[5]);
        }

    }

    void AsignarVertices(Nodo[] puntos)
    {
        for (int i = 0; i < puntos.Length; i++)
        {
            if (puntos[i].indice == -1)
            {
                puntos[i].indice = vertices.Count;
                vertices.Add(puntos[i].posicion);
            }
        }

    }

    void CrearTriangulo(Nodo a, Nodo b, Nodo c)
    {
        triangulos.Add(a.indice);
        triangulos.Add(b.indice);
        triangulos.Add(c.indice);

        Triangulo triangulo = new Triangulo(a.indice, b.indice, c.indice);
        AñadirTrianguloAlDiccionario(triangulo.VertexIndexA, triangulo);
        AñadirTrianguloAlDiccionario(triangulo.VertexIndexB, triangulo);
        AñadirTrianguloAlDiccionario(triangulo.VertexIndexC, triangulo);
    }

    void AñadirTrianguloAlDiccionario(int vertexIndexKey, Triangulo triangulo)
    {
        if (diccionarioTriangulos.ContainsKey(vertexIndexKey))
        {
            diccionarioTriangulos[vertexIndexKey].Add(triangulo);
        }
        else
        {
            List<Triangulo> lt = new List<Triangulo>();
            lt.Add(triangulo);
            diccionarioTriangulos.Add(vertexIndexKey, lt);
        }
    }

    void CalcularBordesMalla()
    {
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkVertices.Contains(vertexIndex))
            {
                int nuevoVerticeBorde = GetVerticesConectadosBorde(vertexIndex);
                if (nuevoVerticeBorde != -1)
                {
                    checkVertices.Add(vertexIndex);

                    List<int> nuevoOutline = new List<int>();
                    nuevoOutline.Add(vertexIndex);
                    outlines.Add(nuevoOutline);
                    FollowOutline(nuevoVerticeBorde, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }

    void FollowOutline(int vertexIndex, int outlineIndex)
    {
        outlines[outlineIndex].Add(vertexIndex);
        checkVertices.Add(vertexIndex);
        int vertexNext = GetVerticesConectadosBorde(vertexIndex);
        if (vertexNext != -1)
        {
            FollowOutline(vertexNext, outlineIndex);
        }
    }

    int GetVerticesConectadosBorde(int vertexIndex)
    {
        List<Triangulo> triangulosConVertices = diccionarioTriangulos[vertexIndex];
        for (int i = 0; i < triangulosConVertices.Count; i++)
        {
            Triangulo triangulo = triangulosConVertices[i];
            for (int j = 0; j < 3; j++)
            {
                int vertexB = triangulo[j];
                if (vertexB != vertexIndex && !checkVertices.Contains(vertexB))
                {
                    if (EsFueraEdge(vertexIndex, vertexB))
                    {
                        return vertexB;
                    }
                }
            }
        }

        return -1;
    }

    bool EsFueraEdge(int vertexA, int vertexB)
    {
        List<Triangulo> triangulosConA = diccionarioTriangulos[vertexA];
        int triangulosCompartidos = 0;
        for (int i = 0; i < triangulosConA.Count; i++)
        {
            if (triangulosConA[i].Contiene(vertexB))
            {
                triangulosCompartidos ++;
                if (triangulosCompartidos>1)
                {
                    break;
                }
            }
        }
        return triangulosCompartidos == 1;
    }

    struct Triangulo
    {
        public int VertexIndexA;
        public int VertexIndexB;
        public int VertexIndexC;
        int[] vertices;
        public Triangulo(int a, int b, int c)
        {

            VertexIndexA = a;
            VertexIndexB = b;
            VertexIndexC = c;

            vertices = new int[3];
            vertices[0] = a;
            vertices[2] = b;
            vertices[2] = c;
        }

        public int this[int i]
        {
            get
            {
                return vertices[i];
            }
        }

        public bool Contiene(int vertice)
        {
            return (vertice == VertexIndexA || vertice == VertexIndexB || vertice == VertexIndexC);
        }
    }
    public class CuadriculaCuadros
    {
        public Cuadro[,] cuadros;

        public CuadriculaCuadros(int[,] mapa, float tamaño)
        {
            int x = mapa.GetLength(0);
            int y = mapa.GetLength(1);
            float anchoMapa = x * tamaño;
            float largoMapa = y * tamaño;
            ControlNodo[,] controlNodos = new ControlNodo[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Vector3 pos = new Vector3(-anchoMapa / 2 + i * tamaño + tamaño / 2, 0, -largoMapa / 2f + j * tamaño + tamaño / 2f);
                    controlNodos[i, j] = new ControlNodo(pos, mapa[i, j] == 1, tamaño);
                }
            }

            cuadros = new Cuadro[x - 1, y - 1];
            for (int i = 0; i < x-1; i++)
            {
                for (int j = 0; j < y-1; j++)
                {
                    cuadros[i, j] = new Cuadro(controlNodos[i, j + 1], controlNodos[i + 1, j + 1], controlNodos[i + 1, j], controlNodos[i, j]);
                }
            }
        }
    }



    public class Cuadro
    {
        public ControlNodo topLeft, topRight, bottomRight, bottomLeft;
        public Nodo centreTop, centreRight, centreBottom, centreLeft;
        public int configuracion;

        public Cuadro (ControlNodo _superiorIzquierda, ControlNodo _superiorDerecha, ControlNodo _inferiorDerecha, ControlNodo _InferiorIzquierda)
        {
            topRight = _superiorDerecha;
            topLeft = _superiorIzquierda;
            bottomRight = _inferiorDerecha;
            bottomLeft = _InferiorIzquierda;

            centreTop = topLeft.derecha;
            centreRight = bottomRight.arriba;
            centreBottom = bottomLeft.derecha;
            centreLeft = bottomLeft.arriba;

            if (topLeft.activo)
                configuracion += 8;
            if (topRight.activo)
                configuracion += 4;
            if (bottomRight.activo)
                configuracion += 2;
            if (_InferiorIzquierda.activo)
                configuracion += 1;
        }
    }
    public class Nodo
    {
        public Vector3 posicion;
        public int indice = -1;

        public Nodo(Vector3 _pos)
        {
            posicion = _pos;
        }
    }

    public class ControlNodo: Nodo
    {
        public bool activo;
        public Nodo arriba, derecha;
        public ControlNodo(Vector3 _pos, bool _activo, float tamañoCuadro) : base(_pos)
        {
            activo = _activo;
            arriba = new Nodo(posicion + Vector3.forward * tamañoCuadro / 2f);
            derecha = new Nodo(posicion + Vector3.right * tamañoCuadro / 2f);
        }
    }
}
