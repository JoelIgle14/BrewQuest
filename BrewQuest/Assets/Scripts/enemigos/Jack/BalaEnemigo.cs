using UnityEngine;

public class BalaEnemigo2 : MonoBehaviour
{
    public float velocidad = 5f;
    public float tiempoDeVida = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, tiempoDeVida);

        // La bala se mueve con fuerza o velocidad directa
        rb.velocity = Vector2.left * velocidad; // o .right según dirección deseada
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jarryanim j = collision.gameObject.GetComponent<jarryanim>();
            if (j != null)
            {
                j.PerderVida();
            }
        }

        // Destruye la bala al tocar cualquier cosa excepto otras balas o enemigos
        if (!collision.gameObject.CompareTag("enemy") &&
            !collision.gameObject.CompareTag("BalaEnemigo"))
        {
            Destroy(gameObject);
        }
    }
}
