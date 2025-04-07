using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash : MonoBehaviour
{
    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashLength = 0.3f;
    public float dashCooldown = 1f;
    private float dashCounter;
    private float dashCooldownCounter;
    public bool isDashing = false;

    private Rigidbody2D body;
    private PlayerMovement move;

    //public bool canMove = true;


    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        HandleDash();
        
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
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(dashLength);
        body.gravityScale = originalGravity;
        isDashing = false;
        dashCooldownCounter = dashCooldown;
    }
}
