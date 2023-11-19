using UnityEngine;

public class MoverCapsula : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float velocidadEsprinte = 10f;
    public float velocidadRotacion = 2f;
    public float alturaSaltoMinima = 8f;
    public float alturaSaltoMaxima = 15f;
    private bool enSuelo;

    private float tiempoDeSalto;
    public float tiempoMaximoDeSalto = 2f;

    private int contadorEspacio = 0;
    private bool puedeVolar = false;

    public Transform firePoint;

    public float fuerzaEmpujeArea = 5f; // Ajusta la fuerza del empuje
    public float radioEmpujeArea = 5f; // Ajusta el radio del área de empuje

    void Update()
    {
        Mover();
        RotarConMouse();
        Saltar();
        HacerDanioEmpujar();
        EmpujarArea();

        // Permitir volar si se presiona la tecla de espacio dos veces consecutivas
        if (Input.GetKeyDown(KeyCode.Space))
        {
            contadorEspacio++;

            if (contadorEspacio == 2)
            {
                puedeVolar = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            contadorEspacio = 0;
            puedeVolar = false;
        }

        // Volar si la capacidad está activada
        if (puedeVolar)
        {
            Volar();
        }
    }

    void Mover()
    {
        float movimientoX = Input.GetAxis("Horizontal");
        float movimientoZ = Input.GetAxis("Vertical");

        float movimientoWASD_X = Input.GetKey("a") ? -1f : (Input.GetKey("d") ? 1f : 0f);
        float movimientoWASD_Z = Input.GetKey("s") ? -1f : (Input.GetKey("w") ? 1f : 0f);

        float totalMovimientoX = movimientoX + movimientoWASD_X;
        float totalMovimientoZ = movimientoZ + movimientoWASD_Z;

        Vector3 movimiento = new Vector3(totalMovimientoX, 0f, totalMovimientoZ);
        movimiento.Normalize();

        float velocidadActual = Input.GetKey(KeyCode.LeftShift) ? velocidadEsprinte : velocidadMovimiento;

        transform.Translate(movimiento * velocidadActual * Time.deltaTime);
    }

    void RotarConMouse()
    {
        if (Input.GetMouseButton(1))
        {
            float rotacionX = Input.GetAxis("Mouse X") * velocidadRotacion;
            float rotacionY = Input.GetAxis("Mouse Y") * velocidadRotacion;

            transform.Rotate(Vector3.up, rotacionX);
            Camera.main.transform.Rotate(Vector3.left, rotacionY);
        }
    }

    void Saltar()
    {
        if (Input.GetKeyUp(KeyCode.Space) && enSuelo)
        {
            float tiempoPresionado = Mathf.Clamp(Time.time - tiempoDeSalto, 0f, tiempoMaximoDeSalto);
            float fuerzaDeSalto = Mathf.Lerp(alturaSaltoMinima, alturaSaltoMaxima, tiempoPresionado / tiempoMaximoDeSalto);

            GetComponent<Rigidbody>().AddForce(Vector3.up * Mathf.Sqrt(fuerzaDeSalto * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            enSuelo = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            tiempoDeSalto = Time.time;
        }
    }

    void Volar()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Acceleration);
    }

    void HacerDanioEmpujar()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit))
            {
                if (hit.transform.CompareTag("Enemigo"))
                {
                    Enemigo enemigo = hit.transform.GetComponent<Enemigo>();
                    if (enemigo != null)
                    {
                        Vector3 direccion = (enemigo.transform.position - transform.position).normalized;
                        enemigo.RecibirDanioEmpujar(20f, direccion * 10f);
                    }
                }
            }
        }
    }

    void EmpujarArea()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radioEmpujeArea);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemigo"))
                {
                    Enemigo enemigo = collider.GetComponent<Enemigo>();
                    if (enemigo != null)
                    {
                        Vector3 direccion = (enemigo.transform.position - transform.position).normalized;
                        enemigo.RecibirDanioEmpujar(15f, direccion * fuerzaEmpujeArea);
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }
}
