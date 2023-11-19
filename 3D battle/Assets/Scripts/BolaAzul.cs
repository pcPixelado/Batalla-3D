using UnityEngine;

public class BolaAzul : MonoBehaviour
{
    private float danio = 50f;

    void OnTriggerEnter(Collider other)
    {
        Enemigo enemigo = other.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            enemigo.RecibirDanio(danio);
            Destroy(gameObject);
        }
    }

    public void SetDanio(float nuevoDanio)
    {
        danio = nuevoDanio;
    }
}
