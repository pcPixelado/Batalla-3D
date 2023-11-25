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

    public CanvasManager canvasManager;

    private Enemigo ultimoEnemigoQueDano;

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

        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= cooldownDuracion)
        {
            puedeRecibirDanio = true;
        }

        if (vidaActual <= 0f)
        {
            DerrotarJugador();
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
                            ultimoEnemigoQueDano = enemigo;
                        }
                    }
                }
            }
        }

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
                            ultimoEnemigoQueDano = enemigo;
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
                        ultimoEnemigoQueDano = enemigo;
                    }
                }
            }
        }
    }

    void OnCollisionStay(Collision collision)
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
                ultimoEnemigoQueDano = enemigo;
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
        return true;
    }

    public void RecibirDanio(float cantidad)
    {
        RecibirDanio(cantidad, 0, Vector3.zero);
    }

    public void RecibirDanio(float cantidad, float fuerzaEmpuje, Vector3 direccion)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(direccion, ForceMode.Impulse);

        if (puedeRecibirDanio)
        {
            vidaActual -= cantidad;
            vidaActual = Mathf.Max(vidaActual, 0f);

            ActualizarBarraDeVida();

            if (vidaActual <= 0f)
            {
                DerrotarJugador();
            }

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
        canvasManager.barraDeVida.fillAmount = vidaActual / vidaMaxima;
    }

    void DerrotarJugador()
    {
        Debug.Log("Jugador MUERTO PA SIEMPRE..... A NO SER");

        if (ultimoEnemigoQueDano != null)
        {
            // Configurar la cámara en el enemigo que derrotó al jugador
            ConfigurarCamara(ultimoEnemigoQueDano.transform);
        }

        // Llamada a la función de respawn
        RespawnJugador();
    }

    // Método de respawn
    void RespawnJugador()
    {
        // Desactivar jugador y ocultar la barra de vida durante el respawn
        gameObject.SetActive(false);
        canvasManager.barraDeVida.fillAmount = 0f;

        // Posicionar al jugador en el punto de respawn
        GameObject puntoRespawn = GameObject.FindGameObjectWithTag("PuntoDeRespawn");
        if (puntoRespawn != null)
        {
            transform.position = puntoRespawn.transform.position;
            transform.rotation = puntoRespawn.transform.rotation;
        }

        // Reiniciar la vida
        vidaActual = vidaMaxima;

        // Reactivar al jugador después de un breve retraso (puedes ajustar esto)
        Invoke("ReactivarJugador", 2f);
    }

    void ConfigurarCamara(Transform objetivo)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            mainCamera.transform.SetParent(objetivo);
            mainCamera.transform.localPosition = new Vector3(0f, 2f, -5f);
            mainCamera.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("La cámara principal es nula");
        }
    }

    void ReactivarJugador()
    {
        // Restaurar la cámara al jugador
        ConfigurarCamara(transform);

        // Reactivar al jugador y actualizar la barra de vida
        gameObject.SetActive(true);
        ActualizarBarraDeVida();
    }

    // Nuevo: Método para reactivar la cámara en el enemigo después del tiempo de reaparición
    void ReactivarEnemigoCamara()
    {
        if (ultimoEnemigoQueDano != null)
        {
            // Restaurar la cámara al enemigo
            ConfigurarCamara(ultimoEnemigoQueDano.transform);
            ultimoEnemigoQueDano = null; // Limpiar referencia al enemigo
        }
    }
}
