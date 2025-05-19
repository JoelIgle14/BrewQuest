using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoWinston : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D rb;
    private Vector2 direction;
    private bool directionSet = false;
    public bool isExploding;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (directionSet)
            rb.velocity = direction * Speed;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = new Vector2(Mathf.Sign(dir.x), 0f);
        directionSet = true;

        // Voltea el sprite dependiendo de la dirección
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * Mathf.Sign(dir.x);
        transform.localScale = newScale;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (isExploding) return;

        isExploding = true;
        rb.velocity = Vector2.zero;

        Destroy(gameObject);
    }


}