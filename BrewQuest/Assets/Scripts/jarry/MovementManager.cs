using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool saltoSolicitado = false;
    private bool dashSolicitado = false;

    private float fuerzaSalto;
    private float fuerzaDash;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    // Opcional: para chequear desde otros scripts si se puede dashear
    public bool puedeDashear => !saltoSolicitado && !dashSolicitado;
}
