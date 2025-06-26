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

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float direccionX = player.transform.position.x - transform.position.x;

            // Solo tomamos dirección horizontal
            Vector2 direccion = (direccionX > 0) ? Vector2.right : Vector2.left;

            rb.velocity = direccion * velocidad;
        }
        else
        {
            // Si no hay jugador, dispara a la izquierda por defecto
            rb.velocity = Vector2.left * velocidad;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
