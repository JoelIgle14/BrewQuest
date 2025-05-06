using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;

    [SerializeField] public float jumpForce = 7f;
    private Rigidbody2D body;

    public bool canMove = true;
    private bool isGrounded = false;
    public float rayCastDistance = 0.2f;
    private int remainingJumps;
    private const int maxJumps = 1;
    [Header("Plataformas Móviles")]
    private Transform currentPlatform = null;

    //scripts
    private NewBehaviourScript hab;
    private dash dash;
    public Transform position;
    private MovementManager manager;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        dash = GetComponent<dash>();
        hab = GetComponent<NewBehaviourScript>();
        position = GetComponent<Transform>();
        manager = GetComponent<MovementManager>();
    }

    void Update()
    {
        HandleFlip();
        HandleGroundCheck();

        // Solicitamos el salto al Manager
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && canMove && !dash.isDashing)
        {
            manager.SolicitarSalto(jumpForce);
        }

        if (hab.canDoubleJump && !isGrounded)
        {
            HandleDoubleJump(); // Permite segundo salto si no estás en el suelo
        }
    }

    void FixedUpdate()
    {
        // Solo puede moverse si no está dashando
        if (canMove && !dash.isDashing)
        {
            Move();
        }
    }

    private void Move()
    {
        //Recoger el input
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)) moveInput = 0f;

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
        if (Input.GetKey(KeyCode.RightArrow))
            transform.localScale = new Vector3(1, 1, 1);
        else if (Input.GetKey(KeyCode.LeftArrow))
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleGroundCheck()
    {
        float halfWidth = 0.625f; // Ajustar esto según el ancho del personaje
        Vector2 leftOrigin = transform.position + Vector3.left * halfWidth + Vector3.down * 1.1f;
        Vector2 rightOrigin = transform.position + Vector3.right * halfWidth + Vector3.down * 1.1f;

        isGrounded = false;

        RaycastHit2D hitLeft = Physics2D.Raycast(leftOrigin, Vector2.down, rayCastDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(rightOrigin, Vector2.down, rayCastDistance);

        if ((hitLeft.collider != null && (hitLeft.collider.CompareTag("Floor") || hitLeft.collider.CompareTag("PlataformaMovil"))) ||
            (hitRight.collider != null && (hitRight.collider.CompareTag("Floor") || hitRight.collider.CompareTag("PlataformaMovil"))))
        {
            isGrounded = true;
            remainingJumps = maxJumps;
        }
    }

    private void OnDrawGizmos()
    {
        float halfWidth = 0.625f;
        float rayLength = rayCastDistance;

        Vector2 leftOrigin = transform.position + Vector3.left * halfWidth + Vector3.down * 1.1f;
        Vector2 rightOrigin = transform.position + Vector3.right * halfWidth + Vector3.down * 1.1f;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(leftOrigin, leftOrigin + Vector2.down * rayLength);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(rightOrigin, rightOrigin + Vector2.down * rayLength);
    }


    private void HandleDoubleJump()
    {
        // Solo permitir el SEGUNDO salto si no estás en el suelo, tienes la habilidad, y tienes 1 salto restante
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0 && canMove && !dash.isDashing)
        {
            //manager.SolicitarSalto(jumpForce); // Solicitar salto al Manager
            manager.ProcesarInputBufferParaSalto();

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
}
