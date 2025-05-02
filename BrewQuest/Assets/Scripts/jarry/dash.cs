using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash : MonoBehaviour
{
    //Variables
    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashLength = 0.3f;
    public float dashCooldown = 1f;
    private float dashCounter;
    private float dashCooldownCounter;
    public bool isDashing = false;

    //scripts
    private Rigidbody2D body;
    private PlayerMovement move;
    private NewBehaviourScript hab;
    public Animator dashBarAnimator;
    private MovementManager manager;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerMovement>();
        hab = GetComponent<NewBehaviourScript>();
        manager = GetComponent<MovementManager>(); // Referencia al manager
    }

    void Update()
    {
        if (hab.canDash)
        {
            HandleDash();
        }

        if (dashCooldownCounter <= 0 && !isDashing)
        {
            dashBarAnimator.SetTrigger("Ready");
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.E) && dashCooldownCounter <= 0 && !isDashing && move.canMove)
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
        // Evitar que el dash se ejecute si el salto ya está en proceso
        if (manager.puedeDashear && !isDashing)
        {
            isDashing = true;
            dashBarAnimator.SetTrigger("StartCooldown");

            float originalGravity = body.gravityScale;
            body.gravityScale = 0f;

            // Usamos el Manager para manejar la fuerza del dash
            manager.SolicitarDash(dashSpeed);

            yield return new WaitForSeconds(dashLength);
            body.gravityScale = originalGravity;
            isDashing = false;
            dashCooldownCounter = dashCooldown;
        }
    }
}
