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

    public Transform firePoint;

    public float fuerzaEmpujeArea = 5f;
    public float radioEmpujeArea = 5f;

    private float vidaMaxima = 100f;
    private float vidaActual;
    private bool puedeRecibirDanio = true;
    private float cooldownTimer = 0f;
    public float cooldownDuracion = 3f;
    public float danioAlJugador = 5f;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    void Update()
    {
        Mover();
        RotarConMouse();
        Saltar();
        HacerDanioEmpujar();
        EmpujarArea();

        // Actualiza el temporizador de cooldown
        cooldownTimer += Time.deltaTime;

        // Puedes ajustar el tiempo de cooldown según tus necesidades
        if (cooldownTimer >= cooldownDuracion)
        {
            puedeRecibirDanio = true;
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

                        if (puedeEmpujar())
                        {
                            enemigo.RecibirDanioEmpujar(10f, direccion * 2f);
                            RecibirDanio(danioAlJugador); // Aplica daño al jugador
                        }
                    }
                }
            }
        }

        // Resto del código...

        if (Input.GetKeyDown(KeyCode.R))
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

                        if (puedeEmpujar())
                        {
                            enemigo.RecibirDanioEmpujar(20f, direccion * 10f);
                            RecibirDanio(danioAlJugador); // Aplica daño al jugador
                        }
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
                        RecibirDanio(danioAlJugador); // Aplica daño al jugador
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

        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Enemigo enemigo = collision.gameObject.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                RecibirDanio(enemigo.dañoAlJugador);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }

    bool puedeEmpujar()
    {
        return true; // Puedes ajustar la lógica según tus necesidades
    }

    public void RecibirDanio(float cantidad)
    {
        if (puedeRecibirDanio)
        {
            vidaActual -= cantidad;
            vidaActual = Mathf.Max(vidaActual, 0f);

            ActualizarBarraDeVida();

            if (vidaActual <= 0f)
            {
                DerrotarJugador();
            }

            // Inicia el cooldown al recibir daño
            IniciarCooldown();
        }
    }

    void IniciarCooldown()
    {
        puedeRecibirDanio = false;
        cooldownTimer = 0f;
    }

    void ActualizarBarraDeVida()
    {
        // Puedes agregar aquí la lógica para actualizar la barra de vida del jugador si la tienes.
    }

    void DerrotarJugador()
    {
        Debug.Log("Jugador derrotado");
        // Implementa aquí cualquier lógica que desees cuando el jugador queda sin vida
    }
}
