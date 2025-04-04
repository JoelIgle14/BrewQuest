using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyvida : MonoBehaviour
{
    public float health = 3.0f;
    private bool golpeado;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject); // o tu lógica de muerte
            //añadir animacion
        }
    }
}