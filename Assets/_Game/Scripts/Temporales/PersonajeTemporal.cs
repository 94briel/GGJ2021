using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeTemporal : MonoBehaviour
{
    public float velocidad = 10;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + (Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal")) * Time.fixedDeltaTime * velocidad);
    }
}
