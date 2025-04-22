using UnityEngine;

public class PinchoDamage : MonoBehaviour
{
    [Header("Knockback")]
    [SerializeField] private Vector2 knockbackForce = new Vector2(5f, 10f);
    [SerializeField] private float knockbackDuration = 0.56f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player == null) return;

            // Determinar direcci�n desde el pincho al jugador
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Crear fuerza en esa direcci�n con intensidad configurada
            Vector2 force = new Vector2(
                Mathf.Sign(direction.x) * knockbackForce.x,
                knockbackForce.y // siempre hacia arriba
            );

            // Aplicar da�o (si ten�s GameManager)
            GameManager.Instance?.PerderVida();

            // Aplicar knockback
            player.ApplyKnockback(force, knockbackDuration);
        }
    }
}
