using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Rigidbody2D body;
    private Animator anim;

    public bool canMove = true;
    private bool isGrounded = false;
    public float rayCastDistance = 0.2f;
    private int remainingJumps;
    private const int maxJumps = 1;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashLength = 0.3f;
    public float dashCooldown = 1f;
    private float dashCounter;
    private float dashCooldownCounter;
    private bool isDashing = false;

    [Header("Plataformas Móviles")]
    private Transform currentPlatform = null;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        body.freezeRotation = true;
    }

    void Update()
    {
        HandleFlip();
        HandleGroundCheck();
        HandleJump();
        HandleDash();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (canMove && !isDashing)
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
        if (hit.collider != null && hit.collider.CompareTag("Floor"))
        {
            isGrounded = true;
            remainingJumps = maxJumps;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0 && canMove)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            remainingJumps--;
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.E) && dashCooldownCounter <= 0 && !isDashing && canMove)
        {
            StartCoroutine(DoDash());
        }

        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }
    }

    private IEnumerator DoDash()
    {
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(dashLength);
        body.gravityScale = originalGravity;
        isDashing = false;
        dashCooldownCounter = dashCooldown;
    }

    private void UpdateAnimator()
    {
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = transform.position - new Vector3(0f, 1.15f);
        Vector2 direction = Vector2.down;
        Gizmos.DrawLine(origin, origin + direction * rayCastDistance);
    }
}
