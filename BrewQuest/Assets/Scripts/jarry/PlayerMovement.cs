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
    public bool isGrounded = false;
    public float rayCastDistance = 0.2f;
    private int remainingJumps;
    private const int maxJumps = 1;

    [Header("Plataformas Móviles")]
    private Transform currentPlatform = null;

    // Scripts
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

        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && hab.canJump && hab.canMove && !dash.isDashing)
        {
            manager.SolicitarSalto(jumpForce);
        }

        if (hab.canDoubleJump && !isGrounded)
        {
            HandleDoubleJump();
        }
    }

    void FixedUpdate()
    {
        if (hab.canMove && !dash.isDashing)
        {
            Move();
        }
    }

    private void Move()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)) moveInput = 0f;

        float moveSpeed = moveInput * speed;

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
        float halfWidth = 0.625f;
        Vector2 leftOrigin = transform.position + Vector3.left * halfWidth + Vector3.down * 1.1f;
        Vector2 rightOrigin = transform.position + Vector3.right * halfWidth + Vector3.down * 1.1f;
        Vector2 centerOrigin = transform.position + Vector3.down * 1.1f;

        isGrounded = false;

        RaycastHit2D hitLeft = Physics2D.Raycast(leftOrigin, Vector2.down, rayCastDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(rightOrigin, Vector2.down, rayCastDistance);
        RaycastHit2D hitCenter = Physics2D.Raycast(centerOrigin, Vector2.down, rayCastDistance);

        if ((hitLeft.collider != null && (hitLeft.collider.CompareTag("Floor") || hitLeft.collider.CompareTag("PlataformaMovil"))) ||
            (hitRight.collider != null && (hitRight.collider.CompareTag("Floor") || hitRight.collider.CompareTag("PlataformaMovil"))) ||
            (hitCenter.collider != null && (hitCenter.collider.CompareTag("Floor") || hitCenter.collider.CompareTag("PlataformaMovil"))))
        {
            isGrounded = true;
            remainingJumps = maxJumps;
        }
    }

    private void HandleDoubleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0 && canMove && !dash.isDashing)
        {
            manager.ProcesarInputBufferParaSalto();
            remainingJumps = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            ApplyDamage(collision.transform);
        }
    }

    private void ApplyDamage(Transform enemyTransform)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PerderVida();
        }

        float direction = (enemyTransform.position.x > transform.position.x) ? -1 : 1;
        Vector2 knockbackForce = new Vector2(5f * direction, 7f);
        ApplyKnockback(knockbackForce, 0.56f);

        //StartCoroutine(ActivateInvulnerability());
    }

    public void ApplyKnockback(Vector2 force, float duration)
    {
            body.velocity = Vector2.zero;
            body.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(DisableMovementForTime(duration));
    }

    //private IEnumerator ActivateInvulnerability()
    //{
    //    //lacosa
    //}

    private IEnumerator DisableMovementForTime(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaMovil"))
        {
            currentPlatform = null;
        }
    }
}
