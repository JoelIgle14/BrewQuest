using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Rigidbody2D body;
    private Animator anim;

    public bool isGrounded = false;
    public float rayCastDistance;
    public bool canmove = true;

    private int remainingJumps;
    private const int maxJumps = 1; // Ahora permite doble salto

    // DASH
    private Vector2 moveInput;
    private float activeMoveSpeed;
    public float dashSpeed;
    public float dashLength = 5f;
    public float dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;

    private Transform currentPlatform = null;
    // Nuevas variables para manejar la plataforma
    private Vector2 platformOffset;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true; // Evita que el personaje rote
        anim = GetComponent<Animator>(); // Asigna el Animator del personaje
        activeMoveSpeed = speed;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canmove)
        {
            Move();

            // DASH
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            moveInput.Normalize();

            if (dashCounter > 0)
            {
                body.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = speed;
                    dashCoolCounter = dashCooldown;
                }
            }
            else
            {
                body.velocity = new Vector2(moveInput.x * activeMoveSpeed, body.velocity.y);

            }
        }

        if (Input.GetKeyDown(KeyCode.E) && dashCoolCounter <= 0 && dashCounter <= 0)
        anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));

        // Volteo del personaje según la dirección del movimiento
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0)
        {
            dashCounter = dashLength;
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

        //Esto es para girarlo
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Detección de suelo con Raycast
        Vector2 raycastorigin = (Vector2)transform.position + new Vector2(0f, -GetComponent<Collider2D>().bounds.extents.y - 0.1f);
        isGrounded = false;

        RaycastHit2D raycastHit2D = Physics2D.Raycast(raycastorigin, Vector2.down, rayCastDistance);
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.tag == "Floor")
        {
            isGrounded = true;
            remainingJumps = maxJumps; // Restablecer los saltos cuando el personaje toque el suelo
        }
        // Doble salto
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            remainingJumps--;
        } 
    }

    private void Move()
    {
        //codigo pa moverse
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
        // Si el jugador está sobre la plataforma, lo movemos con ella
        if (currentPlatform != null)
        {
            // Desplazamos al jugador en el eje X con la plataforma
            horizontalMove += currentPlatform.GetComponent<Rigidbody2D>().velocity.x;
        }

        // Asignamos la velocidad al jugador
        body.velocity = new Vector2(horizontalMove, body.velocity.y);
    }

    // Colisión con plataformas
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.tag == "PlataformaMovil")
        {
            currentPlatform = collision.transform;
            platformOffset = currentPlatform.position - transform.position;
        }


        if (collision.gameObject.tag == "enemy")
        {
            GameManager.Instance.PerderVida();

            Debug.Log("Colisión con enemigo detectada");

            float enemyPositionX = collision.transform.position.x;
            float playerPositionX = transform.position.x;

            if (enemyPositionX > playerPositionX)
            {
                StartCoroutine(DisableMovementForTime(0.56f));
                body.AddForce(new Vector2(-5.0f, 7.0f), ForceMode2D.Impulse);
            }
            else if (enemyPositionX < playerPositionX)
            {
                StartCoroutine(DisableMovementForTime(0.56f));
                body.AddForce(new Vector2(5.0f, 7.0f), ForceMode2D.Impulse);
            }
        }
    }

    // Colisión con plataformas (cuando el jugador sale de la plataforma)
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.tag == "PlataformaMovil")
        {
            currentPlatform = collision.transform;
            platformOffset = currentPlatform.position - transform.position;
        }

    }

    IEnumerator DisableMovementForTime(float time)
    {
        canmove = false;
        Debug.Log("Movimiento desactivado");
        yield return new WaitForSeconds(time);
        canmove = true;
        Debug.Log("Movimiento activado");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = transform.position - new Vector3(0f, 0.88f);
        Vector2 direction = Vector2.down;
        Gizmos.DrawLine(origin, origin + direction * rayCastDistance);
    }
    
}
