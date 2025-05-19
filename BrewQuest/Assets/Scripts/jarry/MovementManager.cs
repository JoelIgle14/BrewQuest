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

    //scripts
    private PlayerMovement pm;
    private dash dish;

    private Queue<KeyCode> inputBuffer;
    public float bufferTiempo = 0.1f;
    private float bufferTimer;

    [Header("Salto Variable")]
    [SerializeField] private float multiplicadorCutJump = 0.5f; // cuánto se recorta si suelta pronto

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;




    private NewBehaviourScript hab;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (inputBuffer.Count == 0)
            {
                inputBuffer.Enqueue(KeyCode.Space);
                bufferTimer = bufferTiempo;
            }
        }

        if (saltoEnProgreso && Input.GetKeyUp(KeyCode.Space))
        {
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * multiplicadorCutJump);
            }
            saltoEnProgreso = false;
        }

        if (inputBuffer.Count > 0)
        {
            bufferTimer -= Time.deltaTime;
            if (bufferTimer <= 0f)
            {
                inputBuffer.Dequeue();
            }
        }

        if (pm.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Solo procesar salto si no estamos dasheando
        if ((coyoteTimeCounter > 0f) && inputBuffer.Count > 0 && !dish.isDashing)
        {
            ProcesarInputBufferParaSalto();
        }
    }

    //void FixedUpdate()
    //{
    //    if (saltoSolicitado)
    //    {
    //        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
    //        saltoSolicitado = false;
    //        saltoEnProgreso = true;
    //    }
    //    else if (dashSolicitado)
    //    {
    //        rb.velocity = new Vector2(transform.localScale.x * fuerzaDash, 0f);
    //        dashSolicitado = false;
    //    }
    //}

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
        if (inputBuffer.Count > 0 && inputBuffer.Peek() == KeyCode.Space && !dish.isDashing)
        {
            if (coyoteTimeCounter > 0f)
            {
                if (hab != null && !hab.canJump)
                    return;

                SolicitarSalto(pm.jumpForce);
                inputBuffer.Dequeue();
            }
            else if (hab != null && hab.canDoubleJump)
            {
                SolicitarSalto(pm.jumpForce);
                inputBuffer.Dequeue();
            }
        }
    }




    public bool puedeDashear => !saltoSolicitado && !dish.isDashing;
}
