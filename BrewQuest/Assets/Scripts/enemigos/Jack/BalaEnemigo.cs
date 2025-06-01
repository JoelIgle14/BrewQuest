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

        // Mover la bala automáticamente
        rb.velocity = Vector2.left * velocidad;  // o Vector2.right según dirección deseada
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player")) || (collision.gameObject.CompareTag("Floor")))
        {
            Destroy(gameObject);
        }

    }
}
