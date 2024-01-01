using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ondaexpansva : MonoBehaviour
{
    public float fuerzaEmpujeArea = 5f;
    public float radioEmpujeArea = 5f;

    private Enemigo ultimoEnemigoQueDano;

    void Start()
    {

    }

    void Update()
    {
        // No incluyas el método EmpujarArea directamente aquí.
    }

    public void ActivarEmpujarArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radioEmpujeArea);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemigo"))
            {
                Enemigo enemigo = collider.GetComponent<Enemigo>();
                if (enemigo != null)
                {
                    Vector3 direccion = (enemigo.transform.position - transform.position).normalized;
                    enemigo.RecibirDanioEmpujar(15f, direccion * fuerzaEmpujeArea);
                    ultimoEnemigoQueDano = enemigo;
                }
            }
        }
    }
}
