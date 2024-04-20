using UnityEngine;

public class Magnet : MonoBehaviour
{
    [Tooltip("Intensidad de la atracción magnética")]
    public float magnetStrength = 1f;

    private void FixedUpdate()
    {
        // Obtener todas las bolas y el punto cero en la escena
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        GameObject puntoCero = GameObject.FindGameObjectWithTag("PuntoCero");

        foreach (GameObject ball in balls)
        {
            // Calcular la dirección de la fuerza magnética hacia el punto cero
            Vector3 direction = puntoCero.transform.position - ball.transform.position;

            // Atraer la bola hacia el punto cero
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.AddForce(direction.normalized * magnetStrength);
        }
    }
}
