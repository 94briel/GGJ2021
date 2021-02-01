using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public static Portal singleton;
    public bool activo;
    public GameObject tripas;
    public string portalPP;

    private void Awake()
    {
        singleton = this;
        portalPP = PlayerPrefs.GetString("nivel");
    }

    public void Activar()
    {
        tripas.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(portalPP, 1);
            SceneManager.LoadScene("Mapa");
        }
    }
}
