using UnityEngine;

public class Meteoro1 : MonoBehaviour
{
    private Enemigo enemigoObjetivo;

    public void EstablecerEnemigoObjetivo(Enemigo enemigo)
    {
        enemigoObjetivo = enemigo;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo") && enemigoObjetivo != null)
        {
            // L�gica para causar da�o al enemigo
            enemigoObjetivo.RecibirDanio(60f);

            // Destruir el meteoro
            Destroy(gameObject);
        }
    }
}
