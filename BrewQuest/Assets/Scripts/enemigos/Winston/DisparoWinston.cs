using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoWinston : MonoBehaviour
{
    public float Speed;

    private Rigidbody2D Rigidbody2D;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

   private void FixedUpdate()
    {
        Rigidbody2D.velocity = Vector2.right * Speed;
    }
}
