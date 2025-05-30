using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class empujecartita : MonoBehaviour
{
    public float golpeForceX = 3f;
    public float golpeForceY = 3f;
    public float golpeDuration = 0.5f;

    private Enemyvida ev;
    private Rigidbody2D rb;
    private PlayerMovement jarry;
    private Cartita ca;
    public bool isBeingHit = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ev = GetComponent<Enemyvida>();
        jarry = FindObjectOfType<PlayerMovement>();
        ca = GetComponent<Cartita>();
    }

    public void EmpujeEnemigo()
    {
        StartCoroutine(GolpeCoroutine());
    }

    IEnumerator GolpeCoroutine()
    {
        float direction = (jarry.transform.position.x > transform.position.x) ? -1f : 1f;
        Vector2 baseDirection = new Vector2(direction, 1f).normalized;
        Vector2 golpeVector = new Vector2(baseDirection.x * golpeForceX, baseDirection.y * golpeForceY);

        rb.velocity = Vector2.zero;
        rb.AddForce(golpeVector, ForceMode2D.Impulse);

        yield return new WaitForSeconds(golpeDuration);

        ca.isBeingHit = false;
        ev.golpeado = false;
    }
}
