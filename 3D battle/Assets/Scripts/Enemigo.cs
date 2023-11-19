using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    void Update()
    {
        // Puedes agregar cualquier lógica adicional que necesites en el Update del enemigo.
    }

    public void RecibirDanio(float cantidad)
    {
        Debug.Log("Recibiendo daño: " + cantidad);
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
        Debug.Log("Recibiendo daño con empuje: " + cantidad);
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
        // Si tienes una barra de vida, puedes actualizarla aquí.
    }

    void DerrotarEnemigo()
    {
        Debug.Log("Enemigo derrotado");
        // Aquí puedes agregar cualquier acción que desees cuando el enemigo es derrotado.
        Destroy(gameObject);
    }
}
