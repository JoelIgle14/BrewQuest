using UnityEngine;
using System.Collections;

public class PinchoDamage : MonoBehaviour
{
    [Header("Knockback")]
    [SerializeField] private Vector2 knockbackForce = new Vector2(5f, 10f);
    [SerializeField] private float knockbackDuration = 0.56f;
    [SerializeField] private float freezeDuration = 0.5f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player == null) return;

            // Determinar dirección desde el pincho al jugador
            Vector2 direction = (player.transform.position - transform.position).normalized;

            Vector2 force = new Vector2(
                Mathf.Sign(direction.x) * knockbackForce.x,
                knockbackForce.y
            );

            // Aplicar daño
            GameManager.Instance?.PerderVida();

            // Congelar posición durante freezeDuration
            StartCoroutine(FreezePlayerTransform(player, freezeDuration));

            // Aplicar knockback
            player.ApplyKnockback(force, knockbackDuration);

            
        }
    }

    private IEnumerator FreezePlayerTransform(PlayerMovement player, float duration)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 originalVelocity = playerRb.velocity;
            playerRb.constraints = RigidbodyConstraints2D.FreezePosition;

            yield return new WaitForSeconds(duration);

            playerRb.constraints = RigidbodyConstraints2D.None;
            playerRb.velocity = originalVelocity;
        }
    }
}
