using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Rigidbody2D body;

    // Salto
    public Transform groundCheck;
    public LayerMask groundLayer;

    // Doble salto
    private bool canDoubleJump;
    private bool isGrounded;
    int saltosrestantes = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true; // Evita que el personaje rote
    }

    private void Update()
    {
        // Verificar si el personaje está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Movimiento horizontal
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        // Restablecer doble salto cuando el jugador toca el suelo
        if (isGrounded)
        {
            canDoubleJump = true;
            saltosrestantes = 0;
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded && saltosrestantes <=2)
            {
                Jump();
                saltosrestantes++;
            }
            else if (canDoubleJump && saltosrestantes < 2) // Si no está en el suelo, permite doble salto
            {
                Jump();
                saltosrestantes++; 
                canDoubleJump = false; // Evita más saltos en el aire

            }
        }
    }

    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
    }



//esto no va
void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION");
        if (collision.gameObject.tag == "enemy")
        {
            Debug.Log("Colisión con enemigo detectada");
            body.velocity = new Vector2(body.velocity.x, body.velocity.x);
            body.AddForce(new Vector2(-1.0f, 40.0f), ForceMode2D.Impulse);
        }
    }

    ////Pintar RayCast
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Vector2 origin = transform.position - new Vector3(0f, 0.51f);
    //    Vector2 direction = Vector2.down;

    //    Gizmos.DrawLine(origin, origin + direction * rayCastDistance);
    //}
}





