using UnityEngine;

public class Bola : MonoBehaviour
{
    private GameObject enemigoMasCercano;
    public float danioBola = 50f; // Cantidad de daño que inflige la bola

    // Método para establecer el enemigo más cercano
    public void SetEnemigoMasCercano(GameObject enemigo)
    {
        enemigoMasCercano = enemigo;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == enemigoMasCercano)
        {
            // Si la bola colisiona con el enemigo, causa daño
            Enemigo enemigo = enemigoMasCercano.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(danioBola);
            }

            // Destruimos la bola después de impactar
            Destroy(gameObject);
        }
    }
}
