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

    private Queue<KeyCode> inputBuffer;
    private float bufferTiempo = 0.2f;

    [Header("Salto Variable")]
    [SerializeField] private float multiplicadorCutJump = 0.5f; // cuánto se recorta si suelta pronto

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputBuffer = new Queue<KeyCode>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputBuffer.Enqueue(KeyCode.Space);
            Invoke(nameof(QuitarAccion), bufferTiempo);
        }

        // Detectar si suelta el botón para cortar el salto
        if (saltoEnProgreso && Input.GetKeyUp(KeyCode.Space))
        {
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * multiplicadorCutJump);
            }
            saltoEnProgreso = false;
        }
    }

    void FixedUpdate()
    {
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
        if (inputBuffer.Count > 0 && inputBuffer.Peek() == KeyCode.Space)
        {
            SolicitarSalto(pm.jumpForce);
            inputBuffer.Dequeue();
        }
    }

    private void QuitarAccion()
    {
        if (inputBuffer.Count > 0)
            inputBuffer.Dequeue();
    }

    public bool puedeDashear => !saltoSolicitado && !dashSolicitado;
}
