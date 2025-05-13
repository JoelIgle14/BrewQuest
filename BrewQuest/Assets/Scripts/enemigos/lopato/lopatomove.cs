using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lopatomove : MonoBehaviour
{
    public float chaseDistance;
    public float speedwalk;
    public float speedrun;
    public float contadorT;
    public float tiempocambio;
    public bool esDerecha;

    // Movimiento especial
    public float fuerzaSalto;
    public float distanciaRodadaObjetivo;
    public float tiempoEsperaDespuesRodar = 1f; // Tiempo de recuperación después de rodar

    // Referencias
    private GameObject target;
    private Vector3 initialScale;
    private Vector2 puntoInicioRodada;
    private Vector2 direccionRodada;
    private bool haSaltado = false;
    private float tiempoRecuperacion = 0f;

    public BoxCollider2D patasCollider;
    private Animator anim;

    // Knockback y control de golpe
    private hitalopato recback;
    private Enemyvida ev;
    public bool isBeingHit = false;

    // Estados
    private enum EstadoEnemigo
    {
        Patrullando,
        PreparadoRodar,
        Rodando,
        Volviendo
    }
    private EstadoEnemigo estado = EstadoEnemigo.Patrullando;

    private void Start()
    {
        target = GameObject.Find("Jarry");
        initialScale = transform.localScale;

        ev = GetComponent<Enemyvida>();
        recback = GetComponent<hitalopato>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (ev.golpeado && !isBeingHit)
        {
            isBeingHit = true;
            recback.EmpujeEnemigo();
        }

        if (!ev.golpeado)
        {
            isBeingHit = false;

            switch (estado)
            {
                case EstadoEnemigo.Patrullando:
                    Patrullar();
                    break;

                case EstadoEnemigo.PreparadoRodar:
                    PrepararRodada();
                    break;

                case EstadoEnemigo.Rodando:
                    Rodar();
                    break;

                case EstadoEnemigo.Volviendo:
                    VolverAPatrulla();
                    break;
            }
        }
    }

    void Patrullar()
    {
        float distancia = Vector2.Distance(transform.position, target.transform.position);

        if (distancia <= chaseDistance)
        {
            estado = EstadoEnemigo.PreparadoRodar;
            haSaltado = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, fuerzaSalto);
        }
        else
        {
            Vector3 newScale = initialScale;

            if (esDerecha)
            {
                transform.position += Vector3.right * speedwalk * Time.deltaTime;
                newScale.x = -Mathf.Abs(initialScale.x);
            }
            else
            {
                transform.position += Vector3.left * speedwalk * Time.deltaTime;
                newScale.x = Mathf.Abs(initialScale.x);
            }

            transform.localScale = newScale;

            contadorT -= Time.deltaTime;
            if (contadorT <= 0)
            {
                contadorT = tiempocambio;
                esDerecha = !esDerecha;
            }
        }
    }

    void PrepararRodada()
    {
        if (!haSaltado && GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            patasCollider.enabled = false;
            anim.SetTrigger("rodar");

            puntoInicioRodada = transform.position;
            direccionRodada = (target.transform.position - transform.position).normalized;

            haSaltado = true;
            estado = EstadoEnemigo.Rodando;
        }
    }

    void Rodar()
    {
        transform.position += (Vector3)direccionRodada * speedrun * Time.deltaTime;

        float distanciaRodada = Vector2.Distance(transform.position, puntoInicioRodada);
        if (distanciaRodada >= distanciaRodadaObjetivo)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, fuerzaSalto);
            estado = EstadoEnemigo.Volviendo;
            tiempoRecuperacion = tiempoEsperaDespuesRodar;
        }
    }

    void VolverAPatrulla()
    {
        if (tiempoRecuperacion <= 0f)
        {
            if (GetComponent<Rigidbody2D>().velocity.y <= 0 && !patasCollider.enabled)
            {
                patasCollider.enabled = true;
                anim.SetTrigger("idlear");
                tiempoRecuperacion = tiempoEsperaDespuesRodar;
            }
            else
            {
                estado = EstadoEnemigo.Patrullando;
            }
        }
        else
        {
            tiempoRecuperacion -= Time.deltaTime;
        }
    }
}
