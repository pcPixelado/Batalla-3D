using UnityEngine;

public class escudomediano : MonoBehaviour
{
    public GameObject escudo; // Asigna el objeto que quieres instanciar en el Inspector
    public Transform firePoint; // Asigna el objeto "fire point" en el Inspector
    public float distanciaDeAparicion = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        // Calcula la posición de aparición a una cierta distancia desde el "fire point"
        Vector3 posicionDeAparicion = firePoint.position + firePoint.forward * distanciaDeAparicion;

        // Instancia el objeto en la posición calculada
        Instantiate(escudo, posicionDeAparicion, Quaternion.identity);
    }
}
