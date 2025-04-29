using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winston : MonoBehaviour
{
    //Ataque
    public GameObject bulletPrefab;
    public Transform player;              // Referencia al jugador
    public Transform firePoint;           // Punto desde donde dispara
    public float detectionRange = 5f;     // Rango de visión
    public float fireRate = 1f;           // Tiempo entre disparos

    private float fireCooldown = 0f;


    public float patrolDistance = 5f;
    public float speed = 2f;

    //logica de golpe
    public float golpeForceX = 3f;
    public float golpeForceY = 3f;
    public float golpeDuration = 0.5f; 

    private bool isBeingHit = false; // Para controlar si está siendo golpeado

    private Vector3 startPosition;
    private int direction = 1;

    //otros scripts o componentes
    private Enemyvida ev;
    private Rigidbody2D rb;
    private PlayerMovement jarry;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        ev = GetComponent<Enemyvida>();
        jarry = FindObjectOfType<PlayerMovement>();

    }

    void Update()
    {
        if (ev.golpeado && !isBeingHit)
        {
            isBeingHit = true; // Activamos que está siendo golpeado
            StartCoroutine(GolpeCoroutine());
        }

        if (!ev.golpeado && !isBeingHit)
        {
            {
                transform.position += Vector3.right * direction * speed * Time.deltaTime;
            }


            //flipear
            if (Mathf.Abs(transform.position.x - startPosition.x) >= patrolDistance)
            {
                direction *= -1; // Cambia la dirección

                //esta línea flipea al colega
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }


        IEnumerator GolpeCoroutine()
        {
            //KnockBack aplicado desde el rigidbody

            float direction = (jarry.transform.position.x > transform.position.x) ? -1f : 1f;

            Vector2 baseDirection = new Vector2(direction, 1f).normalized; // con esta linea va igual de fuerte a los dos lados
            Vector2 golpeVector = new Vector2(baseDirection.x * golpeForceX, baseDirection.y * golpeForceY); //esta es la del empuje

            //reseteo y aplicacion de fuerza
            rb.velocity = Vector2.zero;
            rb.AddForce(golpeVector, ForceMode2D.Impulse);

            yield return new WaitForSeconds(golpeDuration);
   
            isBeingHit = false;
            ev.golpeado = false;
        }

        //Disparo

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (player.position.x > transform.position.x && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = 1f / fireRate;
            }
        }

        fireCooldown -= Time.deltaTime;

    }

    void Shoot()
    {
        // Dirección hacia el jugador
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Crear la bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Enviarle la dirección
        bullet.GetComponent<DisparoWinston>().SetDirection(direction);
    }


}
