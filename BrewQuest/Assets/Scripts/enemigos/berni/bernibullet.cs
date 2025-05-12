using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class berniBullet : MonoBehaviour
{
    public float launchForce = 15f;        // Magnitud de la velocidad inicial
    public float launchAngle = 60f;       // Ángulo del disparo en grados
    private Animator anim;

    private Rigidbody2D rb;
    private bool isExploding = false;
    private bool directionLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Destroy(gameObject, 5f);

        // Calcula dirección desde el ángulo y aplica velocidad inicial
        float angleRad = launchAngle * Mathf.Deg2Rad;
        Vector2 launchDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        if (directionLeft) launchDirection.x *= -1;

        rb.velocity = launchDirection * launchForce;

        // Flip visual si va hacia la izquierda
        if (directionLeft)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    public void setDirection(Vector2 direction)
    {
        directionLeft = direction.x < 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Floor"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (isExploding) return;

        isExploding = true;
        rb.velocity = Vector2.zero;

        if (anim != null)
        {
            anim.SetTrigger("explosion");
        }

        Destroy(gameObject, 0.5f);
    }
}
