using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform respawnPoint; // El punto de respawn donde el jugador aparecer�
    public float respawnDelay = 2f; // Tiempo de retraso antes del respawn

    private void Start()
    {
        // Verifica si no se ha asignado el respawnPoint
        if (respawnPoint == null)
        {
            Debug.LogWarning("Respawn point no asignado. Asigna un punto de respawn en el Inspector.");
        }
    }

    // M�todo para detectar la colisi�n con el vac�o
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FallZone"))  // Aseg�rate de asignar el tag "FallZone" a tu objeto de ca�da
        {
            Die(); // Llama a la funci�n para "matar" al jugador
        }
    }

    // M�todo para "matar" al jugador
    public void Die()
    {
        // Llama a la funci�n Respawn despu�s de un retraso
        Invoke("Respawn", respawnDelay);
    }

    // L�gica para respawn
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
 