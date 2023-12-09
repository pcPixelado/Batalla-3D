using UnityEngine;

public class HabilidadLuna : MonoBehaviour
{
    public GameObject meteoroPrefab;
    public float cooldownDuracion = 60f; // Duración del cooldown en segundos
    private float tiempoUltimoUso;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && PuedeUsarHabilidad())
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
            Instantiate(meteoroPrefab, enemigoCercano.transform.position + Vector3.up * 1000f, Quaternion.identity);

            // Registra el tiempo de uso de la habilidad
            tiempoUltimoUso = Time.time;
        }
    }

    bool PuedeUsarHabilidad()
    {
        // Verifica si ha pasado el tiempo de cooldown
        return Time.time - tiempoUltimoUso >= cooldownDuracion;
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
