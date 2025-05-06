using UnityEngine;

public class Disparo : MonoBehaviour
{
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 10f;
    public float delayEntreDisparos = 0.5f;
    public int maxTiros = 5;

    private NewBehaviourScript habilidades;
    private float tiempoUltimoDisparo = 0f;

    private bool powerUpActivo = false;
    private int tirosDisponibles = 0;

    void Awake()
    {
        habilidades = GetComponent<NewBehaviourScript>();
    }

    void Update()
    {
        // Solo puede disparar si tiene el power-up y le quedan tiros
        if (powerUpActivo && habilidades != null && habilidades.canShoot && Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time > tiempoUltimoDisparo + delayEntreDisparos && tirosDisponibles > 0)
            {
                Disparar();
                tiempoUltimoDisparo = Time.time;
                tirosDisponibles--;
            }
        }
    }

    void Disparar()
    {
        float direccion = transform.localScale.x > 0 ? 1f : -1f;

        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);

        // Paso 3: asignar al jugador como dueño de la bala
        Bala scriptBala = bala.GetComponent<Bala>();
        if (scriptBala != null)
        {
            scriptBala.dueño = gameObject;
        }

        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(direccion * fuerzaDisparo, 0f), ForceMode2D.Impulse);

        if (direccion < 0)
        {
            Vector3 escala = bala.transform.localScale;
            escala.x *= -1;
            bala.transform.localScale = escala;
        }
    }


    // Llama esto cuando recojas el power-up por primera vez
    public void ActivarPowerUp()
    {
        if (!powerUpActivo)
        {
            powerUpActivo = true;
            tirosDisponibles = maxTiros;
        }
    }

    // Llama esto desde otro script cuando golpees a un enemigo
    public void RecargarTiro()
    {
        if (powerUpActivo && tirosDisponibles < maxTiros)
        {
            tirosDisponibles++;
        }
    }
}
