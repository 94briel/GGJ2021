using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoGenerico : MonoBehaviour
{
    public Estado estado;
    public float rangoVision = 2;
    public float rangoEscapar = 3;
    public float rangoAtacar = 1;
    public float daño;
    public Vida vidaEnemigo;
    public EstadoTalisman tipo;

    NavMeshAgent agente;

    float cuadradoRangoVision;
    float cuadradoRangoEscapar;
    float cuadradoRangoAracar;

    IEnumerator Start()
    {
        cuadradoRangoVision = rangoVision * rangoVision;
        cuadradoRangoEscapar = rangoEscapar * rangoEscapar;
        cuadradoRangoAracar = rangoAtacar * rangoAtacar;

        agente = GetComponent<NavMeshAgent>();

        while (estado != Estado.muerto)
        {
            yield return new WaitForSeconds(1);
            switch (estado)
            {
                case Estado.idle:
                    EstadoIdle();
                    break;
                case Estado.siguiendo:
                    EstadoSiguiendo();
                    break;
                case Estado.atacando:
                    EstadoAtacando();
                    break;
                case Estado.muerto:
                    EstadoMuerto(); // Ojo que esta podría no entrar
                    break;
                default:
                    break;
            }
        }
    }

    public void CambiarEstado(Estado e)
    {
        estado = e;
        if (e == Estado.atacando)
        {
            if (vidaEnemigo == null)
            {
                vidaEnemigo = Control.singleton.jugador.GetComponent<Vida>();
            }
        }
    }

    void EstadoIdle()
    {
        if (Control.singleton.estadoTalisman != tipo)
        {
            if ((Control.singleton.jugador.transform.position - transform.position).sqrMagnitude < cuadradoRangoVision)
            {
                CambiarEstado(Estado.siguiendo);
            }
        }
    }
    void EstadoSiguiendo()
    {
        agente.SetDestination(Control.singleton.jugador.transform.position);
        float distan = (Control.singleton.jugador.transform.position - transform.position).sqrMagnitude;
        if (distan>cuadradoRangoEscapar)
        {
            agente.SetDestination(transform.position);
            CambiarEstado(Estado.idle);
        }
        if (distan<cuadradoRangoAracar)
        {
            agente.SetDestination(transform.position);
            CambiarEstado(Estado.atacando);
        }
        if (Control.singleton.estadoTalisman == tipo)
        {
            agente.SetDestination(transform.position);
            CambiarEstado(Estado.idle);
        }
    }
    void EstadoAtacando()
    {
        vidaEnemigo.CausarDaño(daño);
        float distan = (Control.singleton.jugador.transform.position - transform.position).sqrMagnitude;
        if (distan > cuadradoRangoAracar + 1)
        {
            CambiarEstado(Estado.siguiendo);
        }
        if (Control.singleton.estadoTalisman == tipo)
        {
            agente.SetDestination(transform.position);
            CambiarEstado(Estado.idle);
        }
    }
    void EstadoMuerto()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtacar);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVision);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoEscapar);
    }
}

public enum Estado
{
    idle = 0,
    siguiendo = 1,
    atacando = 2,
    muerto = 3
}