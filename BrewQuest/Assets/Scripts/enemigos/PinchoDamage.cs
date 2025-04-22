using UnityEngine;

public class PinchoDamage : MonoBehaviour
{
    [SerializeField] private Vector2 knockbackForce = new Vector2(5f, 10f);
    [SerializeField] private float knockbackDuration = 0.56f;
    [SerializeField] private bool relativeToPlayerFacing = true;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player == null) return;

            // Aplicar daño si hay GameManager
            GameManager.Instance?.PerderVida();

            // Calcular dirección del knockback
            float knockbackDirection = CalculateKnockbackDirection(other, player);

            // Aplicar el knockback
            player.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
        }
    }

    private float CalculateKnockbackDirection(Collision2D collision, PlayerMovement player)
    {
        if (collision.contactCount == 0)
            return (player.transform.position.x > transform.position.x) ? 1 : -1;

        // 1. Dirección basada en el punto de contacto
        Vector2 contactPoint = collision.GetContact(0).point;
        Vector2 collisionDirection = (contactPoint - (Vector2)transform.position).normalized;

        // 2. Opción alternativa: dirección opuesta a la que mira el jugador
        if (relativeToPlayerFacing)
        {
            float playerFacingDirection = Mathf.Sign(player.transform.localScale.x);
            return -playerFacingDirection; // Opuesto a donde mira el jugador
        }

        // 3. Dirección basada en posición relativa
        return (collisionDirection.x > 0) ? 1 : -1;
    }
}