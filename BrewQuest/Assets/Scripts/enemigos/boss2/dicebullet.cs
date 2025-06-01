using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class dicebullet : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direccionInicial = new Vector2(1, 1); // se normaliza
    public LayerMask layerParedes;
    public GameObject efectoExplosion;

    private Rigidbody2D rb;
    private bool explotando = false;
    private bool golpeadoPorJugador = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direccionInicial.normalized * speed;
    }

    void Update()
    {
        if (golpeadoPorJugador && !explotando)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.2f, Time.deltaTime * 10f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (explotando) return;

        // Rebote con paredes
        if (((1 << collision.gameObject.layer) & layerParedes) != 0)
        {
            Vector2 normal = collision.contacts[0].normal;
            rb.velocity = Vector2.Reflect(rb.velocity, normal);
        }

        // Colisión con jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            Explota();
        }

        // Colisión con ataque del jugador
        if (collision.gameObject.CompareTag("AtaqueJugador"))
        {
            StartCoroutine(ExplotaConRetraso());
        }
    }

    private IEnumerator ExplotaConRetraso()
    {
        golpeadoPorJugador = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.2f);
        Explota();
    }

    private void Explota()
    {
        explotando = true;
        if (efectoExplosion != null)
        {
            Instantiate(efectoExplosion, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
