using UnityEngine;

public class BalaEnemigo : MonoBehaviour
{
    public float velocidad = 5f;

    void Update()
    {
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aqu� puedes aplicar da�o al jugador
            Destroy(gameObject);
        }
        else if (!other.CompareTag("enemy") && !other.CompareTag("BalaEnemigo"))
        {
            Destroy(gameObject); // Destruye al tocar otra cosa
        }
    }
}
