using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true; // Evita que el personaje rote
    }


    private void Update()
    {
        // Movimiento horizontal
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        // Solo puede saltar si su velocidad en Y es casi 0 (está en el suelo)
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(body.velocity.y) < 0.01f)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }
}
