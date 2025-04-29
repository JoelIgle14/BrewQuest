using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoWinston : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D rb;
    private Vector2 direction;
    private bool directionSet = false;

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
        direction = dir.normalized;
        directionSet = true;
    }
}
