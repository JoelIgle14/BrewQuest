using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class empujechase : MonoBehaviour
{
    //logica de golpe
    public float golpeForceX = 3f;
    public float golpeForceY = 3f;
    public float golpeDuration = 0.5f;

    //otros scripts o componentes
    private Enemyvida ev;
    private Rigidbody2D rb;
    private PlayerMovement jarry;
    private perseguirmc chase;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ev = GetComponent<Enemyvida>();
        jarry = FindObjectOfType<PlayerMovement>();
        chase = GetComponent<perseguirmc>();
    }

    public void EmpujeEnemigo()
    {
        StartCoroutine(GolpeCoroutine());
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

        chase.isBeingHit = false;
        ev.golpeado = false;
    }
}
