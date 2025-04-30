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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Floor"))
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
