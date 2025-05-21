using UnityEngine;

public class BalaEnemigo2 : MonoBehaviour
{
    public float velocidadMovimiento = 5f;

    void Start()
    {
        Destroy(gameObject, 3f); // Destruye la bala a los 3 segundos
    }

    void Update()
    {
        transform.Translate(Vector2.left * velocidadMovimiento * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aquí puedes aplicar daño al jugador
            Destroy(gameObject);
        }
        else if (!other.CompareTag("enemy") && !other.CompareTag("BalaEnemigo"))
        {
            Destroy(gameObject); // Destruye al chocar con otra cosa
        }
    }
}
