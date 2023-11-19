using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;

    public float da�oAlJugador = 5f;
    public float velocidadMovimiento = 3f;

    private Transform jugador; // Referencia al transform del jugador

    void Start()
    {
        vidaActual = vidaMaxima;
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Asigna la referencia al jugador
    }

    void Update()
    {
        MoverHaciaJugador(); // Llama al nuevo m�todo de movimiento hacia el jugador

        // Puedes agregar cualquier l�gica adicional que necesites en el Update del enemigo.
    }

    void MoverHaciaJugador()
    {
        // Verifica si la referencia al jugador es v�lida
        if (jugador != null)
        {
            // Calcula la direcci�n hacia el jugador
            Vector3 direccion = (jugador.position - transform.position).normalized;

            // Mueve al enemigo en la direcci�n del jugador
            transform.Translate(direccion * velocidadMovimiento * Time.deltaTime);
        }
    }

    public void RecibirDanio(float cantidad)
    {
        Debug.Log("Recibiendo da�o: " + cantidad);
        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0f);

        ActualizarBarraDeVida();

        if (vidaActual <= 0f)
        {
            DerrotarEnemigo();
        }
    }

    public void RecibirDanioEmpujar(float cantidad, Vector3 direccionEmpuje)
    {
        Debug.Log("Recibiendo da�o con empuje: " + cantidad);
        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0f);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(direccionEmpuje, ForceMode.Impulse);

        ActualizarBarraDeVida();

        if (vidaActual <= 0f)
        {
            DerrotarEnemigo();
        }
    }

    void ActualizarBarraDeVida()
    {
        // Si tienes una barra de vida, puedes actualizarla aqu�.
    }

    void DerrotarEnemigo()
    {
        Debug.Log("Enemigo derrotado");
        // Aqu� puedes agregar cualquier acci�n que desees cuando el enemigo es derrotado.
        Destroy(gameObject);
    }
}
