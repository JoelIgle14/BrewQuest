using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool saltoSolicitado = false;
    private bool saltoEnProgreso = false;
    private bool dashSolicitado = false;

    private float fuerzaSalto;
    private float fuerzaDash;

    private PlayerMovement pm;
    private dash dish;
    private NewBehaviourScript hab;

    private Queue<KeyCode> inputBuffer;
    public float bufferTiempo = 0.1f;
    private float bufferTimer;

    [Header("Salto Variable")]
    [SerializeField] private float multiplicadorCutJump = 0.5f;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    private bool coyoteBloqueado = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputBuffer = new Queue<KeyCode>();
        pm = GetComponent<PlayerMovement>();
        dish = GetComponent<dash>();
        hab = GetComponent<NewBehaviourScript>();
    }

    void Update()
    {
        // Input salto
        if (Input.GetKeyDown(KeyCode.Space) && inputBuffer.Count == 0)
        {
            inputBuffer.Enqueue(KeyCode.Space);
            bufferTimer = bufferTiempo;
        }

        // Salto variable (cut jump)
        if (saltoEnProgreso && Input.GetKeyUp(KeyCode.Space))
        {
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * multiplicadorCutJump);
            }
            saltoEnProgreso = false;
        }

        // Tiempo del input buffer
        if (inputBuffer.Count > 0)
        {
            bufferTimer -= Time.deltaTime;
            if (bufferTimer <= 0f)
            {
                inputBuffer.Dequeue();
            }
        }

        // Coyote time logic
        if (pm.isGrounded)
        {
            if (!coyoteBloqueado)
            {
                coyoteTimeCounter = coyoteTime;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            coyoteBloqueado = false; // desbloquear cuando ya no toca el suelo
        }

        // Procesar salto si se puede
        if (coyoteTimeCounter > 0f && inputBuffer.Count > 0 && !dish.isDashing)
        {
            ProcesarInputBufferParaSalto();
        }
    }

    void FixedUpdate()
    {
        if (pm.isKnockedBack) return;

        if (saltoSolicitado)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            saltoSolicitado = false;
            saltoEnProgreso = true;
        }
        else if (dashSolicitado)
        {
            rb.velocity = new Vector2(transform.localScale.x * fuerzaDash, 0f);
            dashSolicitado = false;
        }
    }

    public void SolicitarSalto(float fuerza)
    {
        if (saltoSolicitado || dashSolicitado) return;

        saltoSolicitado = true;
        fuerzaSalto = fuerza;
    }

    public void SolicitarDash(float fuerza)
    {
        if (dashSolicitado || saltoSolicitado) return;

        dashSolicitado = true;
        fuerzaDash = fuerza;
    }

    public void ProcesarInputBufferParaSalto()
    {
        if (inputBuffer.Count == 0 || inputBuffer.Peek() != KeyCode.Space) return;

        // Si está en el suelo o en coyote y puede saltar
        if ((coyoteTimeCounter > 0f) && (hab == null || hab.canJump))
        {
            SolicitarSalto(pm.jumpForce);
            inputBuffer.Dequeue();

            coyoteTimeCounter = 0f;      // Cancelamos el coyote para evitar doble salto
            coyoteBloqueado = true;     // No se vuelve a activar hasta que esté en el aire
        }
        else if (hab != null && hab.canDoubleJump)
        {
            SolicitarSalto(pm.jumpForce);
            inputBuffer.Dequeue();
        }
    }

    public bool puedeDashear => !saltoSolicitado && !dish.isDashing;
}
