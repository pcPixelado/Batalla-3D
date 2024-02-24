using UnityEngine;

public class Bola : MonoBehaviour
{
    private GameObject enemigoMasCercano;
    public float danioBola = 50f; // Cantidad de da�o que inflige la bola

    // M�todo para establecer el enemigo m�s cercano
    public void SetEnemigoMasCercano(GameObject enemigo)
    {
        enemigoMasCercano = enemigo;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == enemigoMasCercano)
        {
            // Si la bola colisiona con el enemigo, causa da�o
            Enemigo enemigo = enemigoMasCercano.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(danioBola);
            }

            // Destruimos la bola despu�s de impactar
            Destroy(gameObject);
        }
    }
}
