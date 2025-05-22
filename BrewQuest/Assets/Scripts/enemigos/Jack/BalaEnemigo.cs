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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PerderVida();
            }
        }

        // Destruye la bala si choca con algo que no sea enemigo o bala enemiga
        if (!collision.gameObject.CompareTag("enemy") &&
            !collision.gameObject.CompareTag("BalaEnemigo"))
        {
            Destroy(gameObject);
        }
    }
}
