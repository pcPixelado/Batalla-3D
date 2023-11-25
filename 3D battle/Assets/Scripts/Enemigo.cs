using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;

    public float da�oAlJugador = 50f;
    public float velocidadMovimiento = 3f;

    private Transform jugador;

    void Start()
    {
        vidaActual = vidaMaxima;
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        MoverHaciaJugador();
    }

    void MoverHaciaJugador()
    {
        if (jugador != null)
        {
            Vector3 direccion = (jugador.position - transform.position).normalized;
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
        // Puedes agregar la l�gica de la barra de vida aqu�.
    }

    void DerrotarEnemigo()
    {
        Debug.Log("Enemigo derrotado");
        Destroy(gameObject);
    }
}
