using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Rigidbody2D body;

    public bool isGrounded = false;
    public float rayCastDistance;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true; // Evita que el personaje rote
    }

    private void Update()
    {
        // Movimiento horizontal

        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
       
        //aqui es donde empieza el raycast
        Vector2 raycastorigin = transform.position - new Vector3(0f, 0.51f);  

        isGrounded = false;

        //Creacion del RayCast
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

    //esto no va
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION");
        if (collision.gameObject.tag == "enemy")
        {
            Debug.Log("Colisión con enemigo detectada");
            body.velocity = new Vector2(body.velocity.x, body.velocity.x);
            body.AddForce(new Vector2(-1.0f, 40.0f), ForceMode2D.Impulse);
        }
    }

    //Pintar RayCast
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = transform.position - new Vector3(0f, 0.51f);
        Vector2 direction = Vector2.down;

        Gizmos.DrawLine(origin, origin + direction * rayCastDistance);
    }
}



