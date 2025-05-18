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

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (CheckpointData.ultimaPosicionCheckpoint.HasValue)
        {
            transform.position = CheckpointData.ultimaPosicionCheckpoint.Value;
            CheckpointData.ultimaPosicionCheckpoint = null; // solo una vez
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
            UpdateRespawnPoint(collision.transform);
        }
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
        GameManager.Instance.canMove = false;

        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(parpadeoIntervalo);
            tiempoTranscurrido += parpadeoIntervalo;
        }

        spriteRenderer.enabled = true;
        GameManager.Instance.canMove = true;
    }
}
