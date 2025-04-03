using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovimiento : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento; // Velocidad del fondo

    private Vector2 offset;
    private Material material;
    private Rigidbody2D jugadorRB; // Para acceder a la velocidad del jugador

    private void Awake()
    {
        material = GetComponent<Renderer>().material; // Usamos Renderer en lugar de SpriteRenderer
        jugadorRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>(); // Asegúrate de que "Jarry" es el tag correcto
    }

    private void Update()
    {
        if (jugadorRB != null) // Evitar errores si no encuentra el jugador
        {
            // Obtenemos la velocidad en el eje X del jugador
            float velocidadJugador = jugadorRB.velocity.x;

            // Solo mover el fondo si el jugador tiene velocidad en X (es decir, se está moviendo)
            if (Mathf.Abs(velocidadJugador) > 0.1f) // Si la velocidad es mayor a un pequeño umbral
            {
                // Calcula el desplazamiento basado en la dirección del movimiento del jugador
                offset = velocidadMovimiento * Mathf.Sign(velocidadJugador) * Time.deltaTime;

                // Aplica el desplazamiento al fondo
                material.mainTextureOffset += offset;
            }
        }
    }
}
