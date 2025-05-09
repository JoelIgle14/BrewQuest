using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winston : MonoBehaviour
{
    //Ataque
    public GameObject bulletPrefab;
    public Transform player;              // Referencia al jugador
    public Transform firePoint;           // Punto desde donde dispara
    public float detectionRange = 5f;     // Rango de visi�n
    public float fireRate = 1f;           // Tiempo entre disparos

    private float fireCooldown = 0f;

    private Vector3 startPosition;
    //private int direction = 1;

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
                fireCooldown = 2f / fireRate;
            }
        }

        fireCooldown -= Time.deltaTime;

    }

    void Shoot()
    {
        // Direcci�n hacia el jugador
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Crear la bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Enviarle la direcci�n
        bullet.GetComponent<DisparoWinston>().SetDirection(direction);
    }


}
