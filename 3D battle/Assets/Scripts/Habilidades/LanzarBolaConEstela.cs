using UnityEngine;

public class LanzarBolaConEstela : MonoBehaviour
{
    public GameObject bolaPrefab; // Prefab de la bola que lanzaremos
    public Transform puntoLanzamiento; // Punto de origen del lanzamiento
    public float fuerzaLanzamiento = 10f; // Fuerza con la que se lanzará la bola
    public float danioBola = 50f; // Cantidad de daño que inflige la bola

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            LanzarBolaHaciaEnemigoMasCercano();
        }
    }

    void LanzarBolaHaciaEnemigoMasCercano()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemigo");

        GameObject enemigoMasCercano = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (GameObject enemigo in enemigos)
        {
            float distancia = Vector3.Distance(puntoLanzamiento.position, enemigo.transform.position);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                enemigoMasCercano = enemigo;
            }
        }

        if (enemigoMasCercano != null)
        {
            GameObject bola = Instantiate(bolaPrefab, puntoLanzamiento.position, Quaternion.identity);
            Rigidbody rb = bola.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Pasamos una referencia al enemigo más cercano a la bola
                Bola bolaScript = bola.GetComponent<Bola>();
                if (bolaScript != null)
                {
                    bolaScript.SetEnemigoMasCercano(enemigoMasCercano);
                }

                Vector3 direccion = (enemigoMasCercano.transform.position - puntoLanzamiento.position).normalized;
                rb.AddForce(direccion * fuerzaLanzamiento, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("El objeto bola no tiene un componente Rigidbody.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontraron enemigos con el tag especificado.");
        }
    }
}
