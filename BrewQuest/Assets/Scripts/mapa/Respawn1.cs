using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform respawnPoint; // El punto actual de respawn
    public float respawnDelay = 2f; // Tiempo de retraso antes del respawn

    private void Start()
    {
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
        else if (collision.CompareTag("Checkpoint"))
        {
            UpdateRespawnPoint(collision.transform);
        }
    }

    public void Die()
    {
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
    }

    void UpdateRespawnPoint(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
        Debug.Log("Checkpoint actualizado: " + newRespawnPoint.position);
    }
}
