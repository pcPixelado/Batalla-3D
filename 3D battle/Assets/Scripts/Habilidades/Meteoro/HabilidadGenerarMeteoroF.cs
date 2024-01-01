using UnityEngine;

public class HabilidadGenerarMeteoroF : MonoBehaviour
{
    public GameObject meteoroPrefab;
    public float cooldownDuracion = 5f; // Duraci�n del cooldown en segundos
    private float tiempoUltimoUso;
    private bool scriptActivo = true; // Variable que controla la activaci�n/desactivaci�n del script

    void Update()
    {
        // Verifica si el script est� activo antes de permitir la ejecuci�n del c�digo
        if (scriptActivo && Input.GetKeyDown(KeyCode.F) && PuedeUsarHabilidad())
        {
            GenerarMeteoro();
        }
    }

    void GenerarMeteoro()
    {
        GameObject enemigoCercano = EncontrarEnemigoCercano();

        if (enemigoCercano != null)
        {
            Instantiate(meteoroPrefab, enemigoCercano.transform.position + Vector3.up * 5f, Quaternion.identity);
            tiempoUltimoUso = Time.time;
        }
    }

    bool PuedeUsarHabilidad()
    {
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

    // M�todo para desactivar el script
    public void DesactivarScript()
    {
        scriptActivo = false;
    }
}
