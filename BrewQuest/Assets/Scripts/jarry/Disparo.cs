using UnityEngine;

public class Disparo : MonoBehaviour
{
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 10f;
    public float delayEntreDisparos = 0.5f; // Tiempo de espera entre disparos (en segundos)

    private NewBehaviourScript habilidades;
    private float tiempoUltimoDisparo = 0f; // Guarda el momento del último disparo

    void Awake()
    {
        habilidades = GetComponent<NewBehaviourScript>();
    }

    void Update()
    {
        // Verifica si puede disparar y si ha pasado el tiempo de delay
        if (habilidades != null && habilidades.canShoot && Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time > tiempoUltimoDisparo + delayEntreDisparos)
            {
                Disparar();
                tiempoUltimoDisparo = Time.time; // Actualiza el momento del último disparo
            }
        }
    }

    void Disparar()
    {
        // Determina dirección: 1 = derecha, -1 = izquierda
        float direccion = transform.localScale.x > 0 ? 1f : -1f;

        // Instancia y aplica fuerza
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(direccion * fuerzaDisparo, 0f), ForceMode2D.Impulse);

        // Flip visual de la bala si va a la izquierda (opcional)
        if (direccion < 0)
        {
            Vector3 escala = bala.transform.localScale;
            escala.x *= -1;
            bala.transform.localScale = escala;
        }
    }
} 