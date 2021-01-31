using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemigoGenerico : MonoBehaviour
{
    public Estado estado;
    public float rangoVision = 2;
    public float rangoEscapar = 3;
    public float rangoAtacar = 1;
    public float daño;
    public Vida vidaEnemigo;
    public EstadoTalisman tipo;

    public Animator animPersonaje;
    public ParticleSystem particulas;
    [Header("Particulas muerte")]
    public GameObject particulasMuerte;
    public Vector3 offsetParticulasMuerte;
    public float tiempoMorir;

    public UnityEvent iniciaSeguir;
    public UnityEvent iniciaIdle;
    public UnityEvent iniciaAtacar;

    NavMeshAgent agente;

    float cuadradoRangoVision;
    float cuadradoRangoEscapar;
    float cuadradoRangoAracar;

    void Start()
    {
        cuadradoRangoVision = rangoVision * rangoVision;
        cuadradoRangoEscapar = rangoEscapar * rangoEscapar;
        cuadradoRangoAracar = rangoAtacar * rangoAtacar;

        agente = GetComponent<NavMeshAgent>();

       
    }

    IEnumerator Interactuar()
    {
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
    private void OnDisable()
    {
        StopCoroutine(Interactuar());
    }

    private void OnEnable()
    {
        StartCoroutine(Interactuar());
    }

    public void CambiarEstado(Estado e)
    {
        estado = e;

        if (animPersonaje!= null)
        {
            animPersonaje.SetInteger("estado", (int)e);
        }

        if (e == Estado.atacando)
        {
            StartCoroutine(MirarEnemigo());
            if (vidaEnemigo == null)
            {
                vidaEnemigo = Control.singleton.jugador.GetComponent<Vida>();
            }
            iniciaAtacar.Invoke();
        }
        else if (e == Estado.idle)
        {
            iniciaIdle.Invoke();
        }
        else if (e == Estado.siguiendo)
        {
            iniciaSeguir.Invoke();
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
        if (distan > cuadradoRangoEscapar)
        {
            agente.SetDestination(transform.position);
            CambiarEstado(Estado.idle);
        }
        if (distan < cuadradoRangoAracar)
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

    IEnumerator MirarEnemigo()
    {
        yield return new WaitForSeconds(0.3f);
        while (estado == Estado.atacando)
        {
            yield return new WaitForSeconds(0.3f);

            transform.LookAt(Control.singleton.jugador.transform.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }
    void EstadoAtacando()
    {
        vidaEnemigo.CausarDaño(daño);
        float distan = (Control.singleton.jugador.transform.position - transform.position).sqrMagnitude;
        if (particulas!= null)
        {
            particulas.Play();
        }

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

    [ContextMenu("Matar")]
    public void Morir()
    {
        if(particulasMuerte!=null)
            Instantiate(particulasMuerte, transform.position + offsetParticulasMuerte, Quaternion.identity);
        CambiarEstado(Estado.muerto);
        Destroy(gameObject, tiempoMorir);

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