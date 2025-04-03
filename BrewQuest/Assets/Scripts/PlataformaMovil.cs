using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform puntoA;      // El primer punto de la plataforma
    public Transform puntoB;      // El segundo punto de la plataforma
    public float velocidad = 2f;  // Velocidad de movimiento de la plataforma

    private bool moviendoseHaciaA = true; // ¿La plataforma se mueve hacia el puntoA o hacia el puntoB?
    private Transform jugador;   // Referencia al jugador
    private Vector2 offset;      // Desplazamiento relativo del jugador con la plataforma

    private void Update()
    {
        MoverPlataforma();  // Mueve la plataforma

        // Si el jugador está encima de la plataforma, se moverá con ella
        if (jugador != null)
        {
            // Mueve al jugador con la plataforma en el eje X
            jugador.position = new Vector2(transform.position.x + offset.x, jugador.position.y);
        }
    }

    private void MoverPlataforma()
    {
        // Movimiento de la plataforma entre puntoA y puntoB
        if (moviendoseHaciaA)
        {
            transform.position = Vector2.MoveTowards(transform.position, puntoA.position, velocidad * Time.deltaTime);
            if (transform.position == puntoA.position)
                moviendoseHaciaA = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, puntoB.position, velocidad * Time.deltaTime);
            if (transform.position == puntoB.position)
                moviendoseHaciaA = true;
        }
    }

    // Cuando el jugador entra en contacto con la plataforma
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Verificamos si el objeto es el jugador
        {
            jugador = collision.transform;  // Asignamos al jugador

            // Guardamos el desplazamiento relativo del jugador respecto a la plataforma
            offset = jugador.position - transform.position;
        }
    }

    // Cuando el jugador sale de la plataforma
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugador = null;  // El jugador ya no está encima de la plataforma
        }
    }
}
