using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicaLanza : MonoBehaviour
{
    public float velocidadCaida = 10f;
    public float tiempoVida = 4f;

    private void Start()
    {
        Destroy(gameObject, tiempoVida); // Se destruye tras cierto tiempo
    }

    void Update()
    {
        transform.Translate(Vector3.down * velocidadCaida * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplica daño al jugador aquí
            Debug.Log("Jugador golpeado por pica");
            Destroy(gameObject);
        }
    }
}

