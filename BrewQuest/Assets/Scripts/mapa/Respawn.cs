using UnityEngine;
using System.Linq;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Transform respawnPoint;
    public float respawnDelay = 2f;

    public float parpadeoTiempo = 2f;
    public float parpadeoIntervalo = 0.2f;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public bool esInvulnerable = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (CheckpointData.ultimaPosicionCheckpoint.HasValue)
        {
            transform.position = CheckpointData.ultimaPosicionCheckpoint.Value;
            CheckpointData.ultimaPosicionCheckpoint = null;
        }

        if (respawnPoint == null)
        {
            Debug.LogWarning("Respawn point no asignado. Asigna un punto de respawn en el Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FallZone"))
        {
            Die();
        }

        if (collision.CompareTag("Checkpoint"))
        {
            // Sumamos puntos si el checkpoint no ha sido activado antes
            Checkpoint checkpoint = collision.GetComponent<Checkpoint>();
            if (checkpoint != null && !checkpoint.yaActivado)
            {
                checkpoint.yaActivado = true;

                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoints(checkpoint.puntosPorCheckpoint);
                }
            }

            // Siempre actualizamos el respawn, se hayan dado puntos o no
            UpdateRespawnPoint(collision.transform);
        }
    }


    public void TakeDamage(Transform enemyTransform)
    {
        if (esInvulnerable) return;

        GameManager.Instance.PerderVida();

        // Aplica knockback a través del PlayerMovement
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            float direction = (enemyTransform.position.x > transform.position.x) ? -1 : 1;
            Vector2 knockbackForce = new Vector2(5f * direction, 7f);
            movement.ApplyKnockback(knockbackForce, 0.56f);
        }

        StartCoroutine(ParpadeoTemporal(parpadeoTiempo));
    }

    public void Die()
    {
        GameManager.Instance.PerderVida();
        Invoke("Respawn", respawnDelay);
    }

    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            Debug.LogError("No se ha asignado un punto de respawn.");
        }

        ReiniciarElementosDelMapa();
        StartCoroutine(ParpadeoTemporal(parpadeoTiempo));
    }

    void UpdateRespawnPoint(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
        Debug.Log("Checkpoint actualizado: " + newRespawnPoint.position);
    }

    void ReiniciarElementosDelMapa()
    {
        foreach (IReiniciable reiniciable in FindObjectsOfType<MonoBehaviour>().OfType<IReiniciable>())
        {
            reiniciable.Reiniciar();
        }
    }

    IEnumerator ParpadeoTemporal(float duracion)
    {
        esInvulnerable = true;

        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(parpadeoIntervalo);
            tiempoTranscurrido += parpadeoIntervalo;
        }

        spriteRenderer.enabled = true;
        esInvulnerable = false;
    }
}
