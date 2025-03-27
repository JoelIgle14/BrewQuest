using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jarrymove : MonoBehaviour
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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true; // Evita que el personaje rote

        anim = GetComponent<Animator>(); // Asigna el Animator del personaje
    }

    private void Update()
    {
        // Movimiento horizontal
        if (canmove)
        {
            Move();
        }

        // Volteo del personaje seg�n la direcci�n del movimiento
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Orientaci�n normal (derecha)
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Se voltea a la izquierda
        }

        // Actualizar el par�metro "Speed" en el Animator
        //anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Detecci�n de suelo con Raycast
        Vector2 raycastorigin = transform.position - new Vector3(0f, 0.86f);
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
    }

    private void Jump()
    {
        isGrounded = true;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }

    // Funci�n de movimiento
    private void Move()
    {
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
    }

    // Colisi�n con enemigos
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Debug.Log("Colisi�n con enemigo detectada");

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
        Vector2 origin = transform.position - new Vector3(0f, 0.86f);
        Vector2 direction = Vector2.down;

        Gizmos.DrawLine(origin, origin + direction * rayCastDistance);
    }
}
