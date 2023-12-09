using UnityEngine;

public class Luna : MonoBehaviour
{
    public float da�o = 90f; // Cantidad de da�o infligida por el meteoro

    void OnTriggerEnter(Collider other)
    {
        // Verifica si ha entrado en contacto con un enemigo
        if (other.CompareTag("Enemigo"))
        {
            // Obt�n el componente de enemigo y aplica el da�o
            Enemigo enemigo = other.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(da�o);
            }

            // Destruye el meteoro al entrar en contacto con un enemigo
            Destroy(gameObject);
        }
    }
}
