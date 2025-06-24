using UnityEngine;

public class dadomov : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        direction = GetRandomDiagonal().normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    private Vector2 GetRandomDiagonal()
    {
        Vector2[] diagonals = new Vector2[]
        {
            new Vector2(1, 1),
            new Vector2(-1, 1),
            new Vector2(1, -1),
            new Vector2(-1, -1)
        };

        return diagonals[Random.Range(0, diagonals.Length)];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("wall") && !collision.collider.CompareTag("Floor"))
            return;

        ContactPoint2D contact = collision.contacts[0];
        Vector2 normal = contact.normal;

        // Rebote horizontal
        if (Mathf.Abs(normal.y) > Mathf.Abs(normal.x))
        {
            direction.y *= -1;
        }
        // Rebote vertical
        else
        {
            direction.x *= -1;
        }

        direction = direction.normalized;

        Debug.Log("Rebote! Nueva dirección: " + direction);
    }
}
