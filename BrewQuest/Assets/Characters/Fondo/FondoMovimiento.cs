using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovimiento : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento; // Velocidad del fondo

    private Vector2 offset;
    private Material material;
    private Rigidbody2D jugadorRB; // Corregido

    private void Awake()
    {
        material = GetComponent<Renderer>().material; // Usamos Renderer en lugar de SpriteRenderer
        jugadorRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (jugadorRB != null) // Evitar errores si no encuentra el jugador
        {
            offset = (jugadorRB.velocity.x / 10f) * velocidadMovimiento * Time.deltaTime;
            material.mainTextureOffset += offset; // Aplica el desplazamiento
        }
    }
}
