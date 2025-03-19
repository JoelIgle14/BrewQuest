using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody2D body;

    // Salto
    public Transform FloorCheck;
    public LayerMask FloorLayer;

    // Control de saltos
    private bool isGrounded;
    [SerializeField] private int saltosRestantes = 2; // Inicialmente, el personaje tiene dos saltos disponibles

    // Tamaño del rectángulo de detección ajustado
    [SerializeField] private Vector2 groundCheckSize = new Vector2(1.0f, 0.2f);

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true; // Evita que el personaje rote
    }

    private void Update()
    {
        // Verificar si el personaje está en el suelo con OverlapBox y Raycast
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapBox(FloorCheck.position, groundCheckSize, 0f, FloorLayer) ||
                     Physics2D.Raycast(FloorCheck.position, Vector2.down, 0.1f, FloorLayer);

        Debug.Log("isGrounded: " + isGrounded + " | saltosRestantes: " + saltosRestantes);

        // Movimiento horizontal
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        // Restablecer saltos cuando el jugador toca el suelo por primera vez
        if (isGrounded && !wasGrounded)
        {
            saltosRestantes = 2;
            Debug.Log("Saltos reiniciados al tocar el suelo.");
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
                saltosRestantes--; // Usa el primer salto
            }
            else if (saltosRestantes > 0) // Si no está en el suelo y aún tiene saltos restantes
            {
                Jump();
                saltosRestantes--; // Usa el segundo salto
            }
        }
    }

    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Dibuja un rectángulo (cubo en 2D) en la posición de FloorCheck
        Gizmos.DrawWireCube(FloorCheck.position, groundCheckSize);
        Gizmos.DrawRay(FloorCheck.position, Vector2.down * 0.3f);
    }
}