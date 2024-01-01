using UnityEngine;

public class Luna : MonoBehaviour
{
    public float daño = 90f; // Cantidad de daño infligida por el meteoro

    void OnTriggerEnter(Collider other)
    {
        // Verifica si ha entrado en contacto con un enemigo
        if (other.CompareTag("Enemigo"))
        {
            // Obtén el componente de enemigo y aplica el daño
            Enemigo enemigo = other.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(daño);
            }

            // Destruye el meteoro al entrar en contacto con un enemigo
            Destroy(gameObject);
        }
    }
}
