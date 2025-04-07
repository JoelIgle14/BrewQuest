using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform respawnPoint; // El punto de respawn donde el jugador aparecerá
    public float respawnDelay = 2f; // Tiempo de retraso antes del respawn

    private void Start()
    {
        // Verifica si no se ha asignado el respawnPoint
        if (respawnPoint == null)
        {
            Debug.LogWarning("Respawn point no asignado. Asigna un punto de respawn en el Inspector.");
        }
    }

    // Método para detectar la colisión con el vacío
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FallZone"))  // Asegúrate de asignar el tag "FallZone" a tu objeto de caída
        {
            Die(); // Llama a la función para "matar" al jugador
        }
    }

    // Método para "matar" al jugador
    public void Die()
    {
        // Llama a la función Respawn después de un retraso
        Invoke("Respawn", respawnDelay);
    }

    // Lógica para respawn
    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position; // Coloca al jugador en el punto de respawn
        }
        else
        {
            Debug.LogError("No se ha asignado un punto de respawn.");
        }
    }
}
 