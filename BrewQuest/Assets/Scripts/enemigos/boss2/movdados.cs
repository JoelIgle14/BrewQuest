using UnityEngine;

public class dadomov : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    private Vector2 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 dir = GetRandomDiagonal().normalized;
        currentVelocity = dir * speed;

        rb.velocity = currentVelocity;
    }

    private Vector2 GetRandomDiagonal()
    {
        Vector2[] diagonals = new Vector2[]
        {
            new Vector2(1,1),
            new Vector2(1,-1),
            new Vector2(-1,1),
            new Vector2(-1,-1)
        };
        return diagonals[Random.Range(0, diagonals.Length)];
    }

    void FixedUpdate()
    {
        // Aseguramos que la velocidad sea la que queremos cada frame
        rb.velocity = currentVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            // Si la normal es casi horizontal (pared vertical)
            if (Mathf.Abs(normal.x) > 0.9f)
            {
                currentVelocity.x = -currentVelocity.x;
            }

            // Si la normal es casi vertical (pared horizontal)
            if (Mathf.Abs(normal.y) > 0.9f)
            {
                currentVelocity.y = -currentVelocity.y;
            }
        }

        // Normalizamos y escalamos para mantener la velocidad constante
        currentVelocity = currentVelocity.normalized * speed;

        // Actualizamos la velocidad del Rigidbody inmediatamente
        rb.velocity = currentVelocity;

        Debug.Log($"Rebote! Nueva velocidad: {currentVelocity}");
    }
}
