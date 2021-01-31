using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeTemporal : MonoBehaviour
{
    public float velocidad = 10;
    public Transform esfera;
    public Transform arma;
    public GameObject bala;
    public Transform modelo;
    Rigidbody rb;
    public Animator animaciones;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Disparar();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        animaciones.SetFloat("velocidad", Vector3.SqrMagnitude(movement));
        rb.MovePosition(transform.position + movement * Time.fixedDeltaTime * velocidad);
        MirarEsfera();
    }

    public void MirarEsfera()
    {
        Vector3 direccion = (esfera.position - transform.position).normalized;
        Vector3 derecha = Vector3.Cross(Vector3.up, direccion);
        Vector3 frente = Vector3.Cross(derecha, Vector3.up);
        modelo.forward = frente;

    }

    public void Disparar()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animaciones.SetTrigger("atacar");
            Instantiate(bala, arma.transform.position, arma.transform.rotation);
        }
    }
}
