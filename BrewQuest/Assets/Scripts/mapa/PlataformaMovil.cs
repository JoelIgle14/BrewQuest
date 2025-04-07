using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    private bool moviendoseHaciaA = true;
    private Transform jugador;
    private Vector3 ultimaPosicion;

    private void Start()
    {
        ultimaPosicion = transform.position;
    }

    private void Update()
    {
        MoverPlataforma();

        if (jugador != null)
        {
            // Mover al jugador según el desplazamiento de la plataforma
            Vector3 desplazamiento = transform.position - ultimaPosicion;
            jugador.position += desplazamiento;
        }

        ultimaPosicion = transform.position;
    }

    private void MoverPlataforma()
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugador = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugador = null;
        }
    }
}
