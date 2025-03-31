using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Rigidbody2D body;
    private Animator anim; // Referencia al Animator

    public bool isGrounded = false;
    public float rayCastDistance;
    public bool canmove = true;

    private int remainingJumps;
    private const int maxJumps = 1;

    //DASH

    //private Rigidbody2D body;
    private Vector2 moveInput;

    private float activeMoveSpeed;
    public float dashSpeed;

    public float dashLength = 5f;
    public float dashCooldown = 1f;

    private float dashCounter;
    private float dashCoolCounter;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true; // Evita que el personaje rote

        anim = GetComponent<Animator>(); // Asigna el Animator del personaje

        activeMoveSpeed = speed;
    }

    private void Update()
    {
        // Movimiento horizontal
        if (canmove)
        {
            Move();
        }

        // Obtener entrada horizontal
        float horizontalInput = Input.GetAxis("Horizontal"); // Se cambia de nombre a horizontalInput

        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Orientación normal (derecha)
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Se voltea a la izquierda
        }

        // Detección de suelo con Raycast
        Vector2 raycastorigin = transform.position - new Vector3(0f, 0.51f);
        isGrounded = false;

        RaycastHit2D raycastHit2D = Physics2D.Raycast(raycastorigin, Vector2.down, rayCastDistance);
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.tag == "Floor")
        {
            isGrounded = true;
            Jump();
            remainingJumps = maxJumps; // Restablecer los saltos cuando el personaje toque el suelo
        }

        // Doble salto
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            remainingJumps--;
        }

        // DASH - Corrección de errores
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0f); // Se asegura de que sea un Vector2
        moveInput.Normalize();

        // Aplicar la velocidad normal o del dash al movimiento
        if (dashCounter > 0)
        {
            body.velocity = new Vector2(transform.localScale.x * dashSpeed, body.velocity.y);
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = speed;
                dashCoolCounter = dashCooldown; // Inicia cooldown
            }
        }
        else
        {
            body.velocity = new Vector2(moveInput.x * activeMoveSpeed, body.velocity.y);
        }

        // Iniciar dash
        if (Input.GetKeyDown(KeyCode.E) && dashCoolCounter <= 0 && dashCounter <= 0)
        {
            dashCounter = dashLength;
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }





    private void Jump()
    {
        isGrounded = true;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }

    // Función de movimiento
    private void Move()
    {
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
    }

    // Colisión con enemigos
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Debug.Log("Colisión con enemigo detectada");

            float enemyPositionX = collision.transform.position.x;
            float playerPositionX = transform.position.x;

            if (enemyPositionX > playerPositionX)
            {
                StartCoroutine(DisableMovementForTime(0.56f)); // Desactivar movimiento
                body.velocity = new Vector2(body.velocity.x, body.velocity.y);
                body.AddForce(new Vector2(-4.0f, 6.0f), ForceMode2D.Impulse);
            }
            else if (enemyPositionX < playerPositionX)
            {
                StartCoroutine(DisableMovementForTime(0.56f)); // Desactivar movimiento
                body.velocity = new Vector2(body.velocity.x, body.velocity.y);
                body.AddForce(new Vector2(4.0f, 6.0f), ForceMode2D.Impulse);
            }
        }
    }

    // Corrutina para desactivar el movimiento por cierto tiempo
    IEnumerator DisableMovementForTime(float time)
    {
        canmove = false; // Desactivar movimiento
        Debug.Log("Movimiento desactivado");
        yield return new WaitForSeconds(time); // Esperar el tiempo indicado
        canmove = true; // Reactivar movimiento
        Debug.Log("Movimiento activado");
    }

    // Pintar RayCast
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = transform.position - new Vector3(0f, 0.51f);
        Vector2 direction = Vector2.down;

        Gizmos.DrawLine(origin, origin + direction * rayCastDistance);
    }
}
