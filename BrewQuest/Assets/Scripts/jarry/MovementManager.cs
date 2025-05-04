using UnityEngine;
using System.Collections.Generic;

public class MovementManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool saltoSolicitado = false;
    private bool dashSolicitado = false;

    private float fuerzaSalto;
    private float fuerzaDash;
    private PlayerMovement pm;

    // Input buffer
    private Queue<KeyCode> inputBuffer;
    private float bufferTiempo = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputBuffer = new Queue<KeyCode>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Guardar salto en el buffer si se presiona espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputBuffer.Enqueue(KeyCode.Space);
            Invoke(nameof(QuitarAccion), bufferTiempo);
        }
    }

    void FixedUpdate()
    {
        if (saltoSolicitado)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            saltoSolicitado = false;
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

    // Llamar a esta función desde otro script justo cuando el jugador pueda saltar
    public void ProcesarInputBufferParaSalto()
    {
        if (inputBuffer.Count > 0 && inputBuffer.Peek() == KeyCode.Space)
        {
            SolicitarSalto(fuerzaSalto);
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
