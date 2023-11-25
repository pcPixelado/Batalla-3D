using UnityEngine;

public class HabilidadGenerarMeteoro : MonoBehaviour
{
    public GameObject meteoroPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GenerarMeteoro();
        }
    }

    void GenerarMeteoro()
    {
        // Encuentra el enemigo más cercano
        GameObject enemigoCercano = EncontrarEnemigoCercano();

        if (enemigoCercano != null)
        {
            // Genera el meteoro sobre el enemigo
            Instantiate(meteoroPrefab, enemigoCercano.transform.position + Vector3.up * 5f, Quaternion.identity);
        }
    }

    GameObject EncontrarEnemigoCercano()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        GameObject enemigoCercano = null;
        float distanciaMinima = float.MaxValue;

        foreach (GameObject enemigo in enemigos)
        {
            float distancia = Vector3.Distance(transform.position, enemigo.transform.position);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                enemigoCercano = enemigo;
            }
        }

        return enemigoCercano;
    }
}
