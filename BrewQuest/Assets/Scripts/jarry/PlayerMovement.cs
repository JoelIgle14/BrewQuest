using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Rigidbody2D body;
    

    public bool canMove = true;
    private bool isGrounded = false;
    public float rayCastDistance = 0.2f;
    private int remainingJumps;
    private const int maxJumps = 1;

    private dash dash;
    public Transform position;

    [Header("Plataformas Móviles")]
    private Transform currentPlatform = null;

    //Habilidades
    private NewBehaviourScript hab;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        dash = GetComponent<dash>();
        hab = GetComponent<NewBehaviourScript>();
        position = GetComponent<Transform>();
    }

    void Update()
    {
        HandleFlip();
        HandleGroundCheck();

        HandleJump(); // Primer salto siempre posible

        if (hab.canDoubleJump)
        {
            HandleDoubleJump(); // Solo activa segundo salto si tienes la habilidad
        }
    }

    void FixedUpdate()
    {
        if (canMove && !dash.isDashing)
        {
            Move();
        }
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float moveSpeed = moveInput * speed;

        // Si estamos sobre plataforma móvil, agregar su velocidad
        if (currentPlatform != null)
        {
            Rigidbody2D platformRb = currentPlatform.GetComponent<Rigidbody2D>();
            if (platformRb != null)
                moveSpeed += platformRb.velocity.x;
        }

        body.velocity = new Vector2(moveSpeed, body.velocity.y);
    }

    private void HandleFlip()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleGroundCheck()
    {
        Vector2 rayOrigin = transform.position - new Vector3(0.0f, 1.15f);
        isGrounded = false;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayCastDistance);
        if (hit.collider != null && (hit.collider.CompareTag("Floor") || hit.collider.CompareTag("PlataformaMovil")))
        {
            isGrounded = true;
            remainingJumps = maxJumps;
        }

    }

    private void HandleJump()
    {
        // Solo permitir el PRIMER salto cuando estás en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canMove)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            remainingJumps = hab.canDoubleJump ? 1 : 0; // Si tiene doble salto, deja 1 para usar luego
        }
    }

    private void HandleDoubleJump()
    {
        // Solo permitir el SEGUNDO salto si no estás en el suelo, tienes la habilidad, y tienes 1 salto restante
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && remainingJumps > 0 && canMove)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            remainingJumps = 0; // Ya no quedan más saltos
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PerderVida();
            }

            float direction = (collision.transform.position.x > transform.position.x) ? -1 : 1;
            body.velocity = new Vector2(0, 0);
            body.AddForce(new Vector2(5f * direction, 7f), ForceMode2D.Impulse);
            StartCoroutine(DisableMovementForTime(0.56f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaMovil"))
        {
            currentPlatform = collision.transform;
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PerderVida();
            }

            float direction = (collision.transform.position.x > transform.position.x) ? -1 : 1;
            body.velocity = new Vector2(0, 0);
            body.AddForce(new Vector2(5f * direction, 7f), ForceMode2D.Impulse);
            StartCoroutine(DisableMovementForTime(0.56f));
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaMovil"))
        {
            currentPlatform = null;
        }
    }

    IEnumerator DisableMovementForTime(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public void ApplyKnockback(Vector2 force, float duration)
    {
        body.velocity = Vector2.zero;
        body.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(DisableMovementForTime(duration));
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = transform.position - new Vector3(0f, 1.15f);
        Vector2 direction = Vector2.down;
        Gizmos.DrawLine(origin, origin + direction * rayCastDistance);
    }
}
