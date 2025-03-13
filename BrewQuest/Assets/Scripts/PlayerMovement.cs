using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Rigidbody2D body;
    private Animator anim; // Referencia al Animator

    public bool isGrounded = false;
    public float rayCastDistance;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true; // Evita que el personaje rote

        anim = GetComponent<Animator>(); // Asigna el Animator del personaje
    }

    private void Update()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(moveInput * speed, body.velocity.y);

        // Girar el personaje según la dirección del movimiento
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Orientación normal (derecha)
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Se voltea a la izquierda
        }


        // Actualizar el parámetro "Speed" en el Animator
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Detección de suelo con Raycast
        Vector2 raycastorigin = transform.position - new Vector3(0f, 0.51f);  
        isGrounded = false;

        RaycastHit2D raycastHit2D = Physics2D.Raycast(raycastorigin, Vector2.down, rayCastDistance); 
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.tag == "Floor")  
        {
            isGrounded = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
            }
        }
    }

    // Pintar RayCast
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = transform.position - new Vector3(0f, 0.51f);
        Vector2 direction = Vector2.down;

        Gizmos.DrawLine(origin, origin + direction * rayCastDistance);
    }
}
